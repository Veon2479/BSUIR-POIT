const express = require('express');
const cookieParser = require('cookie-parser');
const jwt = require('jsonwebtoken');
const mysql = require('mysql');
require("dotenv").config();

const PORT = 3001;

const app = express();
app.use(cookieParser());
app.use(express.json());

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


const verifyUserToken = (req, res, next) => {
   if (req.cookies === undefined || req.cookies.JWT === undefined) {
      return res.status(401).send("Unauthorized request");
   }
   const token = req.cookies.JWT;
   try {
      const decoded = jwt.verify(token, process.env.JWT_SECRET);
      req.login = decoded.username;
      next();
   } catch (err) {
      return res.status(400).send("Invalid token.");
   }
};

function query(sql, args) {
   return new Promise((resolve, reject) => {
      db.query(sql, args, (err, rows) => {
         if (err) return reject(err);
         resolve(rows);
      });
   });
}

app.post('/registration', async (req, res) => {
   const {username, hash} = req.body;

   //check if this login exists in inner db
   let isExists = false;

   const queryExists = 'SELECT * FROM Users WHERE login = ?';
   const valuesExists = [username];
   const rows = await query(queryExists, valuesExists);
   if (rows.length > 0)
      isExists = true;

   let isValid = true;
   if (username.length < 8)
      isValid = false;

   if (!isExists) {
      if (isValid) {
         const queryInsert = 'INSERT INTO Users (login, hash) VALUES (?, ?)';
         const valuesInsert = [username, hash];
         await query(queryInsert, valuesInsert);

         const token = jwt.sign({username}, process.env.JWT_SECRET, {expiresIn: '1h'});
         await res.cookie('JWT', token, {httpOnly: true, maxAge: 3600000, path: "/", secure: false});
         await res.status(200).send({message: 'Registered successfully'});
      } else {
         await res.status(400).send({message: 'Invalid credentials'});
      }
   } else {
      await res.status(400).send({message: 'Login exists'});
   }
});

app.post('/login', async (req, res) => {
   console.log(`here`);

   const {username, hash} = req.body;

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
   }

   if (isValid) {
      const token = await jwt.sign({username}, process.env.JWT_SECRET, {expiresIn: '1h'});
      await res.cookie("JWT", token,{httpOnly: true, maxAge: 3600000, path: "/", secure: false})
          .status(200).send({message: 'Logged in successfully'});
   } else {
      await res.status(401).send({message: 'Invalid credentials'});
   }

});

app.post("/logout", verifyUserToken, async (req, res) => {
   await res.clearCookie("JWT").status(200).send();
});

app.get('/notes/:id', verifyUserToken, async (req, res) => {
   const login = req.login;
   const id = req.params.id;
   const queryGet = `SELECT * FROM Notes INNER JOIN Users ON Notes.idAuthor = Users.idUser WHERE Users.login = ? AND Notes.idNote = ?;`;
   const valuesGet = [login, id];
   const note = await query(queryGet, valuesGet).catch(async () => {
      await res.status(400).send({message: "Cannot get note"});
   });
   if (note !== undefined && note.length > 0 ) {
      await res.status(200).send(note[0]);
   } else {
      await res.status(404).send();
   }
})

app.get('/notes', verifyUserToken, async (req, res) => {
   const login = req.login;
   const queryGet = `SELECT * FROM Notes WHERE idAuthor = (SELECT idUser FROM Users WHERE login = ?) `;
   const valuesGet = [login];
   try {
      const notes = await query(queryGet, valuesGet)
      await res.status(200).send(notes);
   } catch (err) {
      res.status(400).send({message: "Cannot get notes"});
   }
})

app.delete('/notes/:id', verifyUserToken, async (req, res) => {
   const login = req.login;
   const id = req.params.id;
   const queryDelete = `DELETE FROM Notes ` +
       `WHERE idNote = ? ` +
       `AND idAuthor = (SELECT idUser FROM Users WHERE login = ?);`
   const valuesDelete = [id, login];
   await query(queryDelete, valuesDelete).catch(async () => {
      await res.status(400).send({message: "Cannot delete note"}).send();
   });
   await res.status(200).send();
})


app.patch('/notes/:id', verifyUserToken, async (req, res) => {
   const idNote = req.params.id;
   const login = req.login;
   const {title, body} = req.body;

   const queryPatch = `UPDATE Notes SET title = ?, body = ?` +
       ` WHERE idNote = ? ` +
       ` AND idAuthor = (SELECT idUser FROM Users WHERE login = ?) ;`;
   const valuesPatch = [title, body, idNote, login];
   await query(queryPatch, valuesPatch).catch(async () => {
      await res.status(400).send({message: "Cannot patch note"});
   }).then(() => {
      res.status(200).send();
   });

})


app.post('/notes', verifyUserToken, async (req, res) => {
   const queryPost = `INSERT INTO Notes (idAuthor, title, body) ` +
       ` SELECT idUser, ?, ? FROM Users WHERE login = ? ;`;
   const {title, body} = req.body;
   const login = req.login;
   const valuesPost = [title, body, login];
   await query(queryPost, valuesPost).catch(async () => {
      await res.status(400).send({message: "Cannot add new note"});
   });
   await res.status(200).send();
})


app.listen(PORT, () => {

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


