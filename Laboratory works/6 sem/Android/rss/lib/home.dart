// ignore_for_file: import_of_legacy_library_into_null_safe

import 'dart:developer';

import 'package:cached_network_image/cached_network_image.dart';
import 'package:flutter/material.dart';
import 'package:dart_rss/dart_rss.dart';
import "package:http/http.dart" as http;
import 'package:rss/webview.dart';

class Home extends StatefulWidget {
  @override
  State<StatefulWidget> createState() => _Home();
}

class _Home extends State<Home> {
  static const String url = 'https://www.nasa.gov/rss/dyn/breaking_news.rss';
  RssFeed? _feed;
  static const String placeholderImg = 'assets/rickroll.png';

  late GlobalKey<RefreshIndicatorState> _refreshKey;

  Future<void> openFeed(String url) async {
    log(url);
    await Navigator.push(
        context, MaterialPageRoute(builder: (context) => WebView(url)));
  }

  Future<RssFeed> loadFeed() async {
    try {
      final client = http.Client();
      final response = await client.get(Uri.parse(url));
      return RssFeed.parse(response.body);
    } catch (e) {
      //
    }
    return const RssFeed();
  }

  load() async {
    loadFeed().then((result) {
      if (result == const RssFeed()) {
        //TODO: show info aput error!
        return;
      }
      updateFeed(result);
    });
  }

  updateFeed(feed) {
    setState(() {
      _feed = feed;
    });
  }

  @override
  void initState() {
    super.initState();
    _refreshKey = GlobalKey<RefreshIndicatorState>();
    load();
  }

  title(title) {
    return Text(
      title,
      style: const TextStyle(fontSize: 18.0, fontWeight: FontWeight.w500),
      maxLines: 2,
      overflow: TextOverflow.ellipsis,
    );
  }

  subtitle(subTitle) {
    return Text(
      subTitle,
      style: const TextStyle(fontSize: 14.0, fontWeight: FontWeight.w100),
      maxLines: 1,
      overflow: TextOverflow.ellipsis,
    );
  }

  thumbnail(imageUrl) {
    return Padding(
      padding: const EdgeInsets.only(left: 15.0),
      child: CachedNetworkImage(
        placeholder: (context, url) => Image.asset(placeholderImg),
        imageUrl: imageUrl,
        height: 50,
        width: 70,
        alignment: Alignment.center,
        fit: BoxFit.fill,
      ),
    );
  }

  rightIcon() {
    return const Icon(
      Icons.keyboard_arrow_right,
      color: Colors.grey,
      size: 30.0,
    );
  }

  list() {
    return ListView.builder(
      itemCount: _feed?.items.length,
      itemBuilder: (BuildContext context, int index) {
        final item = _feed!.items[index];
        return ListTile(
          title: title(item.title),
          subtitle: subtitle(item.pubDate),
          leading: thumbnail(item.enclosure?.url),
          trailing: rightIcon(),
          contentPadding: const EdgeInsets.all(5.0),
          onTap: () => openFeed(item.link!),
        );
      },
    );
  }

  isFeedEmpty() {
    return null == _feed?.items;
  }

  @override
  Widget build(BuildContext context) {
    return isFeedEmpty()
        ? const Center(
            child: CircularProgressIndicator(),
          )
        : RefreshIndicator(
            key: _refreshKey,
            child: list(),
            onRefresh: () async => load(),
          );
  }
}
