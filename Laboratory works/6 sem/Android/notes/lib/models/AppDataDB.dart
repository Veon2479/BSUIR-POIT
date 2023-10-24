import 'dart:ffi';
import 'dart:io';
import 'dart:convert';
import 'package:flutter_test_app/models/IDatabase.dart';
import 'package:flutter_test_app/models/Note.dart';
import 'package:path_provider/path_provider.dart';

class AppDataDB implements IDatabase {
  static List<int> ids = List.empty(growable: true);
  static String? path;
  static late Directory folder;

  static String getPath(String file) {
    return "$path/$file";
  }

  static File getFile(String file) {
    return File(getPath(file));
  }

  @override
  closeDB() {}

  @override
  initDB() async {
    if (path == null) {
      String appdir = (await getApplicationDocumentsDirectory()).path;
      path = "$appdir/notes";
      folder = Directory(path!);
      if (!await folder.exists()) {
        await folder.create(recursive: true);
      }
    }
  }

  Future<List<int>> getIds() async {
    var res = List<int>.empty(growable: true);
    var items = folder.listSync(followLinks: false, recursive: false);
    for (var item in items) {
      var str = item.path.split('/').last.split('.').first;
      res.add(int.parse(str));
    }
    return res;
  }

  @override
  Future<int> deleteNote(int id) async {
    var ids = await getIds();
    if (ids.contains(id)) {
      var file = getFile("$id.txt");
      await file.delete();
    }
    return 0;
  }

  @override
  Future<List<Map<String, dynamic>>> getAllNotes() async {
    var ids = await getIds();
    var res = List<Map<String, dynamic>>.empty(growable: true);
    for (int i = 0; i < ids.length; i++) {
      var item = await getNote(i);
      if (item != null) {
        res.add(item);
      }
    }
    return res;
  }

  @override
  Future<Map<String, dynamic>?> getNote(int id) async {
    var ids = await getIds();
    if (!ids.contains(id)) {
      return <String, dynamic>{};
    } else {
      var file = getFile("$id.txt");
      String str = await file.readAsString();
      var obj = jsonDecode(str);
      var note = Note();
      note.id = obj['id'];

      var tmp = obj['name'].cast<int>();
      tmp.removeWhere((int x) => x == 0);
      note.name = String.fromCharCodes(tmp);

      tmp = obj['content'].cast<int>();
      tmp.removeWhere((int x) => x == 0);
      note.content = String.fromCharCodes(tmp);

      note.tsCreated =
          DateTime.fromMillisecondsSinceEpoch(obj['tsCreated'] * 1000);
      note.tsUpdated =
          DateTime.fromMillisecondsSinceEpoch(obj['tsUpdated'] * 1000);
      return note.toMap();
    }
  }

  @override
  Future<int> insertNote(Note note) async {
    var ids = await getIds();
    int newId = 0;
    if (ids.isNotEmpty) {
      newId = ids[0];
      for (int i = 0; i < ids.length; i++) {
        if (ids[i] > newId) {
          newId = ids[i];
        }
        newId++;
      }
    }
    note.id = newId;
    String str = jsonEncode(note.toMap());
    var file = getFile("$newId.txt");
    await file.writeAsString(str);
    return 0;
  }

  @override
  Future<int> updateNote(Note note) async {
    return insertNote(note);
  }
}
