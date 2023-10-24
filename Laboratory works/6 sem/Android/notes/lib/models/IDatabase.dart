import 'Note.dart';

abstract class IDatabase {
  initDB() async {
    throw UnimplementedError();
  }

  Future<int> insertNote(Note note) async {
    throw UnimplementedError();
  }

  Future<int> updateNote(Note note) async {
    throw UnimplementedError();
  }

  Future<List<Map<String, dynamic>>> getAllNotes() async {
    throw UnimplementedError();
  }

  Future<Map<String, dynamic>?> getNote(int id) async {
    throw UnimplementedError();
  }

  Future<int> deleteNote(int id) async {
    throw UnimplementedError();
  }

  closeDB() async {
    throw UnimplementedError();
  }
}
