import 'package:flutter_test_app/models/AppDataDB.dart';
import 'package:flutter_test_app/models/IDatabase.dart';
import 'package:flutter_test_app/models/SQLiteDB.dart';

enum ModeDB { SQlite, files }

mixin dbHelper {
  static IDatabase getDbViaMode(ModeDB mode) {
    late IDatabase db;
    switch (mode) {
      case ModeDB.SQlite:
        db = SQLiteDB();
        break;
      case ModeDB.files:
        db = AppDataDB();
        break;
      default:
        throw Exception("No db info provided for note editor");
    }
    return db;
  }

  static String getShowableDbName(ModeDB mode) {
    String res = "";
    switch (mode) {
      case ModeDB.SQlite:
        res = "SQLite";
        break;
      case ModeDB.files:
        res = "Files";
        break;
      default:
        throw Exception("No db info provided for note editor");
    }
    return res;
  }
}
