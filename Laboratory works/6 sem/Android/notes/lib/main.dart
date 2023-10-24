import 'package:flutter/material.dart';

import './screens/home.dart';

void main() => runApp(const MyApp());

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
        debugShowCheckedModeBanner: false,
        title: "Notes",
        home: const Home(),
        theme: ThemeData(
            primaryColor: const Color(0x000000ff),
            scaffoldBackgroundColor: const Color(0x000000ff)));
  }
}
