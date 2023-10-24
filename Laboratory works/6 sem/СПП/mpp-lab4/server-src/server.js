const express = require('express');
const app = express();

const cookieParser = require('cookie-parser');
const jwt = require('jsonwebtoken');
const mysql = require('mysql');
require("dotenv").config();

const PORT = 3001;

const server = require('http').Server(app);

app.use(cookieParser());
app.use(express.json());

const io = require('socket.io')(server, {
   cors: {
      origin: "http://localhost:3000",
      methods: ["GET", "POST", "PUT", "DELETE", "PATCH"],
      allowedHeaders: ["*"],
      credentials: true
   }
});

const db = mysql.createConnection({
   host: '192.168.56.103',
   user: 'andmin',
   database: 'db_notes',
   password: '1234567890',
});

async function allowCrossDomain(req, res, next) {
   await res.header('Access-Control-Allow-Origin', 'http://localhost:3000');
   await res.header('Access-Control-Allow-Methods', 'GET,PUT,POST,DELETE,PATCH');
   await res.header('Access-Control-Allow-Headers', 'Content-Type');
   await res.header('Access-Control-Allow-Credentials', 'true');
   next();
}
app.use(allowCrossDomain);
function query(sql, args) {
   return new Promise((resolve, reject) => {
      db.query(sql, args, (err, rows) => {
         if (err) return reject(err);
         resolve(rows);
      });
   });
}
io.on('connection', (socket) => {
   const verifyUserToken = (req) => {
      if (req.cookies === undefined || req.cookies.JWT === undefined) {
         socket.emit("Unauthorized");
         return false;
      }
      const token = req.cookies.JWT;
      try {
         const decoded = jwt.verify(token, process.env.JWT_SECRET);
         req.login = decoded.username;
         return true;
      } catch (err) {
         socket.emit("Unauthorized");
         return false;
      }
   }

   socket.on('registration', async (req) => {
      console.log("started registration");
      let errors = {};
      const {username, hash} = req; //.body?
      //check if this login exists in inner db
      let isExists = false;
      const queryExists = 'SELECT * FROM Users WHERE login = ?';
      const valuesExists = [username];
      const rows = await query(queryExists, valuesExists);
      if (rows.length > 0) {
         isExists = true;
         errors.loginExists = "This login is already in use";
      }
      let isValid = true;
      if (username.length < 8) {
         isValid = false;
         errors.loginLength = "Login's length must be at least 8 symbols"
      }

      if (!isExists && isValid) {
         const queryInsert = 'INSERT INTO Users (login, hash) VALUES (?, ?)';
         const valuesInsert = [username, hash];
         await query(queryInsert, valuesInsert);
         const token = jwt.sign({username}, process.env.JWT_SECRET, {expiresIn: '1h'});
         const cookieHeader = {
            name: "JWT",
            value: token,
            options: {
               maxAge: 3600000, path: "/", secure: false, //secure: true, sameSite: false
            }
         };
         socket.emit('set-cookie', cookieHeader);
      } else {
         socket.emit("Errors", errors);
      }
      console.log("finished registration");
   });
   socket.on('login', async (req) => {
      console.log("started login");
      let errors = {};
      const {username, hash} = req; //.body?
      let isValid = false;
      const queryExists = 'SELECT * FROM Users WHERE login = ? AND hash = ?';
      const valuesExists = [username, hash];
      const rowsExists = await query(queryExists, valuesExists);
      if (rowsExists.length > 1) {
         const queryDelete = 'DELETE FROM Users WHERE login = ?';
         const valuesDelete = [username];
         await query(queryDelete, valuesDelete);
      }
      if (rowsExists.length === 1) {
         isValid = true;
      } else {
         errors.missingLogin = "No such login";
      }
      if (isValid) {
         const token = await jwt.sign({username}, process.env.JWT_SECRET, {expiresIn: '1h'});
         const cookieHeader = {
            name: "JWT",
            value: token,
            options: {
               maxAge: 3600000, path: "/", secure: false, //secure: true, sameSite: false
            }
         };
         socket.emit('set-cookie', cookieHeader);
      } else {
         return socket.emit("Errors", errors);
      }
      console.log("finished login");
   });
   socket.on('logout', async (req) => {
      console.log("started logout");
      if(!verifyUserToken(req)) return;
      socket.emit('clear-cookie', "JWT");
      console.log("finished logout");
   });
   socket.on('createNote', async (req) => {
      console.log("started creating");

      if(!verifyUserToken(req)) return;
      const queryPost = `INSERT INTO Notes (idAuthor, title, body) ` +
          ` SELECT idUser, ?, ? FROM Users WHERE login = ? ;`;
      const {title, body, login} = req;
      const valuesPost = [title, body, login];
      await query(queryPost, valuesPost).catch(async () => {
         socket.emit("Cannot create new note");
      });
      io.emit('update');
      socket.emit('Note was created');

      console.log("finished creating");
   });
   socket.on('getAllNotes', async (req) => {

      if(!verifyUserToken(req)) return;
      const login = req.login;
      const queryGet = `SELECT * FROM Notes WHERE idAuthor = (SELECT idUser FROM Users WHERE login = ?) `;
      const valuesGet = [login];
      const notes = await query(queryGet, valuesGet)
      io.emit('update');
      socket.emit("getAllNotes", notes);

   });
   socket.on('getNote', async (req) => {
      console.log("started getting note");

      if(!verifyUserToken(req)) return;
      const login = req.login;
      const id = req.id;
      const queryGet = `SELECT * FROM Notes INNER JOIN Users ON Notes.idAuthor = Users.idUser WHERE Users.login = ? AND Notes.idNote = ?;`;
      const valuesGet = [login, id];
      const note = await query(queryGet, valuesGet).catch(async () => {
      });
      if (note !== undefined && note.length > 0 ) {
         io.emit('update');
         socket.emit("getNote", note[0]);
      } else {
         socket.emit("cannot getNote");
      }

      console.log("finished getting note");

   });
   socket.on('deleteNote', async (req) => {
      console.log("started deleting note");

      if(!verifyUserToken(req)) return;
      const login = req.login;
      const id = req.id;
      const queryDelete = `DELETE FROM Notes ` +
          `WHERE idNote = ? ` +
          `AND idAuthor = (SELECT idUser FROM Users WHERE login = ?);`
      const valuesDelete = [id, login];
      await query(queryDelete, valuesDelete).catch(async () => {
         socket.emit("Cannot deleteNote");
      }).then(() => {
         io.emit('update');
         socket.emit('deleteNote');
      });

      console.log("finished deleting note");

   });
   socket.on('patchNote', async (req) => {
      console.log("started patching note");

      if(!verifyUserToken(req)) return;
      const idNote = req.id;
      const login = req.login;
      const {title, body} = req;
      const queryPatch = `UPDATE Notes SET title = ?, body = ?` +
          ` WHERE idNote = ? ` +
          ` AND idAuthor = (SELECT idUser FROM Users WHERE login = ?) ;`;
      const valuesPatch = [title, body, idNote, login];
      await query(queryPatch, valuesPatch).catch(async () => {
         socket.emit("Cannot patchNote");
      }).then(() => {
         io.emit('update');
         socket.emit("patchNote");
      });

      console.log("finished patching note");

   });
});
server.listen(PORT, () => {

   db.connect(async function (err) {
      if (err) {
         console.log("ERR: while connecting to db");
         process.exit(-1);
      } else {
         console.log("LOG: db connected");
         const rows = await query('SHOW TABLES', []);
         console.log(rows);
      }
   });
   console.log("LOG: server was started!");
});