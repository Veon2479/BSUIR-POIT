import 'dart:developer';

import 'package:flutter/material.dart';
import 'package:flutter_test_app/models/IDatabase.dart';
import 'package:flutter_test_app/models/SQLiteDB.dart';
import 'package:flutter_test_app/models/Note.dart';
import 'package:flutter_test_app/models/dbHelper.dart';
import 'colors.dart';
import 'package:zooper_flutter_encoding_utf16/zooper_flutter_encoding_utf16.dart';

class Editor extends StatefulWidget {
  ModeDB modeDB;
  dynamic noteItem;

  Editor({required this.modeDB, this.noteItem}) : super();

  _Editor createState() => _Editor(modeDB: modeDB, noteItem: noteItem);
}

class _Editor extends State<Editor> {
  ModeDB modeDB;
  late IDatabase db;

  dynamic noteItem;

  _Editor({required this.modeDB, this.noteItem}) : super() {
    db = dbHelper.getDbViaMode(modeDB);
    noteContent = "";
    noteTitle = "";
    _isHaveToSave = false;
  }

  final coder = UTF16LE();

  late String noteTitle;
  late String noteContent;

  final TextEditingController titleController = TextEditingController();
  final TextEditingController contentController = TextEditingController();

  void handleTitleChange() {
    setState(() {
      _isHaveToSave = true;
      noteTitle = titleController.text.trim();
    });
  }

  void handleContentChange() {
    setState(() {
      _isHaveToSave = true;
      noteContent = contentController.text.trim();
    });
  }

  saveNote() async {
    setState(() {
      _isHaveToSave = false;
    });
    if (noteTitle.length + noteContent.length != 0) {
      Note note = Note(
        name: noteTitle,
        content: noteContent,
      );
      if (noteItem != null) {
        note.id = noteItem['id'];
        note.tsCreated =
            DateTime.fromMillisecondsSinceEpoch(noteItem['tsCreated'] * 1000);
        note.tsUpdated = DateTime.now();
      }
      try {
        log("Trying save note..");
        await db.initDB();
        log("Db is opened");
        await db.insertNote(note);
        log("Note saved succesfully");
      } catch (e) {
        log("Exception while saving note: ${e.toString()}");
      }
    }
  }

  deleteNote() async {
    try {
      log("Trying delete note..");
      await db.initDB();
      log("Db is opened");
      await db.deleteNote(noteItem['id']);
      log("Note deleted succesfully");
    } catch (e) {
      log("Exception while deleting note: ${e.toString()}");
    }
  }

  handleDeleteAction() {
    deleteNote();
    Navigator.of(context).pop(true);
    setState(() {});
  }

  void handleBackArrow() {
    Navigator.pop(context);
  }

  bool _isHaveToSave = false;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        backgroundColor: clBackground,
        appBar: AppBar(
          backgroundColor: clMain,
          leading: IconButton(
            icon: const Icon(
              Icons.arrow_back,
              color: clMainContrast,
            ),
            tooltip: 'Back',
            onPressed: () => {handleBackArrow()},
          ),
          actions: [
            IconButton(
                onPressed: () async =>
                    {_isHaveToSave ? await saveNote() : null},
                icon: Icon(
                  Icons.save,
                  color: (_isHaveToSave ? clMainContrast : clBackground),
                )),
            IconButton(
              onPressed: () => {noteItem != null ? handleDeleteAction() : null},
              icon: Icon(
                Icons.delete,
                color: (noteItem != null ? clMainContrast : clBackground),
              ),
            )
          ],
          title: TextField(
            maxLines: 1,
            decoration: const InputDecoration(
                hintText: 'Note\'s title', border: InputBorder.none),
            controller: titleController,
          ),
        ),
        body: Padding(
          padding: const EdgeInsets.symmetric(vertical: 0.0, horizontal: 10.0),
          child: Stack(children: [
            TextField(
              maxLines: null,
              keyboardType: TextInputType.multiline,
              decoration: const InputDecoration(
                  hintText: 'Place for your note', border: InputBorder.none),
              controller: contentController,
            ),
          ]),
        ));
  }

  @override
  void initState() {
    super.initState();
    if (noteItem != null) {
      titleController.text = coder.decode(noteItem["name"]);
      contentController.text = coder.decode(noteItem["content"]);
      noteContent = contentController.text.trim();
      noteTitle = titleController.text.trim();
    } else {
      titleController.text = "";
      contentController.text = "";
    }
    titleController.addListener(handleTitleChange);
    contentController.addListener(handleContentChange);
  }

  @override
  void dispose() {
    titleController.dispose();
    contentController.dispose();
    super.dispose();
  }
}
