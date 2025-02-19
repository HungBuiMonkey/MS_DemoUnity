using JetBrains.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataModel
{
    [Serializable]
    public class SyncData
    {
        public string w;
        public int ts;
        public int te;
        public int s;
        public int e;
    }

    [Serializable]
    public class Audio
    {
        public int id;
        [JsonProperty("tag_title", NullValueHandling = NullValueHandling.Ignore)] public string tag_title;
        public List<SyncData> sync_data;
        public int duration;
        [JsonProperty("name_original", NullValueHandling = NullValueHandling.Ignore)] public string name_original;
        public string voices_id;
        [JsonProperty("voices_type_id", NullValueHandling = NullValueHandling.Ignore)] public int voices_type_id;
        public string link;
        public string file_path;
        [JsonProperty("score", NullValueHandling = NullValueHandling.Ignore)] public int score;
    }

    [Serializable]
    public class Image
    {
        public int id;
        public string file_type;
        public string title;
        public int images_categories_id;
        public int text_group_id;
        public string link;
        public string file_path;
    }

    [Serializable]
    public class Gaf
    {
        public int id;
        public string link;
        public string file_path;
    }

    [Serializable]
    public class Video
    {
        public int id;
        public int duration;
        public string description;
        public string tag_title;
        public int video_category_id;
        public string link;
        public string file_path;
        public List<VideoInteractive> interactive;
    }

    [Serializable]
    public class VideoInteractive
    {
        [JsonProperty("name")] public string name;
        [JsonProperty("maker_name")] public string makerName;
        [JsonProperty("in")] public string inTime;
        [JsonProperty("out")] public string outTime;
        [JsonProperty("touch_vector")] public string touchVector;
    }

    [Serializable]
    public class Phonic
    {
        public List<int> audio;
    }

    [Serializable]
    public class Word
    {
        public int word_id;
        public int text_id;
        public string path_word;
        public string path_bundle;
        public string text;
        public string name_display;
        public int sentence_type;
        public List<Image> image;
        [CanBeNull] public List<Gaf> gaf;
        public List<Video> video;
        public List<Audio> audio;
        public List<Audio> audio_effect;
        public List<Spine> spine;
        public ColorWord color;
        public List<FilterWord>? filter_word;
        public List<Phonic> phonic;
        public List<int>? list_not_game;
        public bool? isSelected = false;
        public int? word_type;
        public List<string>? syllables;
        public List<string>? audioSyllables;
        public bool? isSentence;
        public List<Word> listWordSentence;
        public Color textColor = Color.clear;
        public List<PhonicPosition>? phonic_position;
    }
    [Serializable]
    public class Spine
    {
        [JsonProperty("id")] public string? id;
        [JsonProperty("link")] public string? link;
        [JsonProperty("file_path")] public string? file_path;
    }

    [Serializable]
    public class PhonicPosition
    {
        [JsonProperty("name")] public string? name;
        [JsonProperty("position")] public string? position;
    }

    [Serializable]
    public class ColorWord
    {
        public string? text;
        public string? highlight;
    }

    [Serializable]
    public class FilterWord
    {
        public string? text;
        public string? name_display;
        public int word_id;
        public string? path_word;
        public string? path_bundle;
    }

    [Serializable]
    public class Sentence
    {
        public int word_id;
        public string text;
        public string? path_word;
        public List<int>? list_not_game;
        public int? type_sentence;
        public List<FilterWord>? filter_word;
    }

    [Serializable]
    public class ListNumberWord
    {
        public int? word_id;
        public int? number;
    }

    [Serializable]
    public class ListWord
    {
        public int word_id;
        public string text;
        public string? path_word;
        public List<int>? list_not_game;
        public List<Sentence>? sentence;
        public int? repeats_word;
        public List<int>? list_flow;
        public string? name_c;
        public bool? checkFlow;
    }

    [Serializable]
    public class ListWordBk
    {
        public int word_id;
        public string text;
        public string path_word;
        public List<int>? list_not_game;
        public List<Sentence>? sentence;
        public int? repeats_word;
        public List<int>? list_flow;
    }

    [Serializable]
    public class LessonData
    {
        public List<ListWord>? list_word;
        public List<ListWordBk>? list_word_bk;
        public string? flow;
        public int? key_flow;
        public List<ListNumberWord>? list_number_word;
    }
}

