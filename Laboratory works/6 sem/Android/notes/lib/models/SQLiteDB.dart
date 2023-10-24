import 'package:flutter_test_app/models/Note.dart';
import 'package:sqflite/sqflite.dart';
import 'IDatabase.dart';
import 'package:mutex/mutex.dart';

class SQLiteDB implements IDatabase {
  static const _name = "SQLiteNotesDatabase.db";
  static const _version = 1;

  late Database database;
  static const tableName = 'notes';

  static int cnt = 0;
  final m = Mutex();

  @override
  initDB() async {
    await m.acquire();
    try {
      cnt++;
      database = await openDatabase(_name, version: _version,
          onCreate: (Database db, int version) async {
        await db.execute('''CREATE TABLE $tableName (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    name TEXT,
                    content TEXT,
                    tsCreated INTEGER,
                    tsUpdated INTEGER
                    )''');
      });
    } finally {
      m.release();
    }
  }

  @override
  closeDB() async {
    await m.acquire();
    try {
      cnt--;
      if (cnt <= 0) {
        await database.close();
      }
    } finally {
      m.release();
    }
  }

  @override
  Future<int> deleteNote(int id) async {
    return await database.delete(tableName, where: 'id = ?', whereArgs: [id]);
  }

  @override
  Future<List<Map<String, dynamic>>> getAllNotes() async {
    return await database.query(tableName);
  }

  @override
  Future<Map<String, dynamic>?> getNote(int id) async {
    var result =
        await database.query(tableName, where: 'id = ?', whereArgs: [id]);

    if (result.isNotEmpty) {
      return result.first;
    }

    return null;
  }

  @override
  Future<int> insertNote(Note note) async {
    return await database.insert(tableName, note.toMap(),
        conflictAlgorithm: ConflictAlgorithm.replace);
  }

  @override
  Future<int> updateNote(Note note) async {
    return await database.update(tableName, note.toMap(),
        where: 'id = ?',
        whereArgs: [note.id],
        conflictAlgorithm: ConflictAlgorithm.replace);
  }
}
