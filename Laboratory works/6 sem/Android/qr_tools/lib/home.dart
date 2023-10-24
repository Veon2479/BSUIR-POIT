import 'package:flutter/material.dart';
import 'package:qr_tools/generate.dart';
import 'package:qr_tools/scan.dart';

class Home extends StatefulWidget {
  const Home({super.key});

  @override
  State<Home> createState() => _HomeState();
}

class _HomeState extends State<Home> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
          title: const Text("QR tools"),
          centerTitle: true,
        ),
        body: Container(
            padding: const EdgeInsets.all(50.0),
            child: Column(
              mainAxisAlignment: MainAxisAlignment.center,
              crossAxisAlignment: CrossAxisAlignment.stretch,
              children: <Widget>[
                const Image(
                    image: NetworkImage(
                        "http://www.rocketfarmstudios.com/wp-content/uploads/2015/08/wallpaper_qr_code_jesus_by_existcze-d5krc2g.jpg")),
                textButton("Scan QR", ScanPage()),
                const SizedBox(
                  height: 2.0,
                ),
                textButton("Generate QR", GeneratePage()),
              ],
            )));
  }

  Widget textButton(String text, Widget widget) {
    return TextButton(
      onPressed: () {
        Navigator.of(context)
            .push(MaterialPageRoute(builder: ((context) => widget)));
      },
      child: Text(text),
    );
  }
}
