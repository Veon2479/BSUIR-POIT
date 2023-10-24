import 'dart:convert';

import 'package:zooper_flutter_encoding_utf16/zooper_flutter_encoding_utf16.dart';

class Note {
  int? id;
  String name;
  String content;
  DateTime? tsCreated;
  DateTime? tsUpdated;

  final encoder = UTF16LE();

  // Note(this.id, this.name, this.content, this.tsCreated, this.tsUpdated);
  Note({
    this.name = "New Note",
    this.content = "",
  }) {
    tsCreated = DateTime.now();
    tsUpdated = tsCreated;
  }

  Map<String, dynamic> toMap() {
    var data = {
      'id': id,
      'name': encoder.encode(name),
      'content': encoder.encode(content),
      'tsCreated': tsCreated!.millisecondsSinceEpoch ~/ 1000,
      'tsUpdated': tsUpdated!.millisecondsSinceEpoch ~/ 1000,
    };

    return data;
  }

  @override
  String toString() {
    return {
      'id': id,
      'name': utf8.encode(name),
      'content': utf8.encode(content),
      'tsCreated': tsCreated!.millisecondsSinceEpoch ~/ 1000,
      'tsUpdated': tsUpdated!.millisecondsSinceEpoch ~/ 1000,
    }.toString();
  }
}
