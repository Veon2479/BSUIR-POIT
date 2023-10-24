import 'dart:async';
import 'dart:developer' as dlp;
import 'dart:math';
import 'package:dart_numerics/dart_numerics.dart';
import 'package:flutter/material.dart';
import 'package:carousel_slider/carousel_slider.dart';
import 'package:flutter_test_app/models/IDatabase.dart';
import 'package:flutter_test_app/models/dbHelper.dart';
import 'editor.dart';
import 'colors.dart';
import 'package:zooper_flutter_encoding_utf16/zooper_flutter_encoding_utf16.dart';

class Home extends StatefulWidget {
  const Home({super.key});
  @override
  _HomeState createState() => _HomeState();
}

class _HomeState extends State<Home> {
  Map<ModeDB, List<Map<String, dynamic>>> notes = {};

  static late _HomeState self;

  _HomeState() {
    ModeDB.values.forEach((mode) {
      notes[mode] = <Map<String, dynamic>>[];
    });
    self = this;
  }

  ModeDB curMode = ModeDB.SQlite;

  void onUpdate() {
    setState(() {});
  }

  final TextEditingController filterController = TextEditingController();
  String filter = "";

  void handleFilterChange() {
    setState(() {
      filter = filterController.text.trim();
      dlp.log(filterController.text.trim());
    });
  }

  @override
  void initState() {
    super.initState();
    filterController.addListener(handleFilterChange);
  }

  @override
  void dispose() {
    filterController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final decoder = UTF16LE();
    return MaterialApp(
      title: 'Notes',
      home: Scaffold(
        backgroundColor: clBackground,
        appBar: AppBar(
            automaticallyImplyLeading: false,
            backgroundColor: clMain,
            leading: const Icon(
              Icons.note,
              color: clMainContrast,
            ),
            title: TextField(
              maxLines: 1,
              decoration: InputDecoration(
                hintText: 'Search filter',
                border: InputBorder.none,
                suffixIcon: IconButton(
                  onPressed: () {
                    filterController.clear();
                  },
                  icon: const Icon(
                    Icons.clear,
                    color: clMainContrast,
                  ),
                ),
              ),
              controller: filterController,
            )),
        body: CarouselSlider.builder(
          itemCount: ModeDB.values.length,
          itemBuilder: (BuildContext context, int itemIndex, int pageIndex) =>
              Column(
            mainAxisSize: MainAxisSize.max,
            children: [
              Text(ModeDB.values.elementAt(itemIndex).toString()),
              Expanded(
                  child: FutureBuilder<List<Map<String, dynamic>>>(
                future: getAllNotes(ModeDB.values.elementAt(itemIndex)),
                builder: (context, snapshot) {
                  if (snapshot.hasData) {
                    return ListView.builder(
                      itemCount: snapshot.data?.length,
                      itemBuilder: (context, index) {
                        dynamic item = snapshot.data?[index];
                        String name =
                            decoder.decode(item['name']).toLowerCase();
                        if (name.contains(filter.toLowerCase())) {
                          return DisplayNote(
                            note: item,
                            modeDB: curMode,
                          );
                        } else {
                          return Container();
                        }
                      },
                      shrinkWrap: true,
                    );
                  } else if (snapshot.hasError) {
                    return const Text(
                        "Some errors occured while loading notes..");
                  } else {
                    return const Center(
                      child: CircularProgressIndicator(backgroundColor: clMain),
                    );
                  }
                },
              ))
            ],
          ),
          options: CarouselOptions(
            height: MediaQuery.of(context).size.height - 100.0,
            enlargeStrategy: CenterPageEnlargeStrategy.height,
            enlargeCenterPage: true,
            animateToClosest: true,
            autoPlay: false,
            initialPage: 0,
            enableInfiniteScroll: false,
            scrollDirection: Axis.horizontal,
            onPageChanged: (index, reason) {
              curMode = ModeDB.values[index];
            },
          ),
        ),
        floatingActionButton: FloatingActionButton(
          tooltip: 'New Note',
          backgroundColor: clMain,
          onPressed: () async {
            await Navigator.push(
                context,
                MaterialPageRoute(
                    builder: (context) => Editor(
                          modeDB: curMode,
                        ))).then(
              (value) {
                setState(() {});
              },
            );
          },
          child: const Icon(
            Icons.add,
            color: clMainContrast,
          ),
        ),
      ),
    );
  }

  Future<List<Map<String, dynamic>>> getAllNotes(ModeDB modeDB) async {
    IDatabase db = dbHelper.getDbViaMode(modeDB);
    int id = Random().nextInt(0xFFFFFFFF);
    try {
      dlp.log(
          "$id: Trying to get all notes in db: ${dbHelper.getShowableDbName(modeDB)}");
      await db.initDB();
      List<Map> notesList = await db.getAllNotes();
      List<Map<String, dynamic>> notesData =
          List<Map<String, dynamic>>.from(notesList);
      dlp.log("$id: ${notesData.length} note(s) got succesfully!");
      return notesData;
    } catch (e) {
      dlp.log("$id: Caught exception while trying access db: ${e.toString()}");
    }
    return List<Map<String, dynamic>>.empty();
  }
}

class DisplayNote extends StatelessWidget {
  final dynamic note;
  final ModeDB modeDB;
  const DisplayNote({Key? key, this.note, required this.modeDB})
      : super(key: key);

  @override
  Widget build(BuildContext context) {
    final decoder = UTF16LE();
    return Padding(
        padding: const EdgeInsets.symmetric(horizontal: 8.0, vertical: 2.0),
        child: Material(
            color: clMain,
            clipBehavior: Clip.hardEdge,
            borderRadius: BorderRadius.circular(5.0),
            child: InkWell(
              onTap: () async {
                await Navigator.push(
                    context,
                    MaterialPageRoute(
                        builder: (context) => Editor(
                              modeDB: modeDB,
                              noteItem: note,
                            ))).then((value) {
                  _HomeState.self.onUpdate();
                });
              },
              onLongPress: () async {
                var db = dbHelper.getDbViaMode(modeDB);
                await db.initDB();
                await db.deleteNote(note['id']).then((value) {
                  _HomeState.self.onUpdate();
                });
              },
              child: Row(
                children: [
                  Expanded(
                    flex: 1,
                    child: Column(
                      mainAxisAlignment: MainAxisAlignment.center,
                      crossAxisAlignment: CrossAxisAlignment.center,
                      mainAxisSize: MainAxisSize.min,
                      children: [
                        Container(
                          alignment: Alignment.center,
                          decoration: const BoxDecoration(
                            color: clBackground,
                            shape: BoxShape.circle,
                            // border: Border.all(),
                          ),
                          child: Padding(
                            padding: const EdgeInsets.all(10),
                            child: Text(
                                getPreview((decoder.decode(note['name'])), 1)
                                    .toUpperCase(),
                                style: const TextStyle(
                                  color: clMainContrast,
                                  fontSize: 21,
                                )),
                          ),
                        )
                      ],
                    ),
                  ),
                  Expanded(
                      flex: 4,
                      child: Column(
                        mainAxisAlignment: MainAxisAlignment.spaceAround,
                        crossAxisAlignment: CrossAxisAlignment.start,
                        mainAxisSize: MainAxisSize.min,
                        children: [
                          Text(
                            getPreview(decoder.decode(note['name']), 15,
                                end: ".."),
                            style: const TextStyle(
                              color: clMainContrast,
                              fontSize: 18,
                              fontWeight: FontWeight.bold,
                            ),
                          ),
                          Container(
                            // decoration: BoxDecoration(border: Border.all()),
                            height: 3,
                          ),
                          Text(
                            getPreview(decoder.decode(note['content']), 20,
                                end: ".."),
                            style: const TextStyle(
                              color: clMainContrast,
                              fontSize: 16,
                              fontWeight: FontWeight.w300,
                            ),
                          )
                        ],
                      ))
                ],
              ),
            )));
  }

  String getPreview(String src, int count, {String end = ""}) {
    String res = "";
    String tmp = src.split("\n")[0];
    if (tmp.length < count) {
      res = tmp;
      if (tmp.length != src.length) res += end;
    } else {
      res = tmp.substring(0, count) + end;
    }
    if (res == "") res = "?";
    return res;
  }
}
