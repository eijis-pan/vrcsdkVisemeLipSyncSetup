# vrcsdkVisemeLipSyncSetup

Unity の Editor 拡張

## Description

VRChat用カスタムアバターとしてアップロードするモデルに Lip Sync を設定する作業を自動化します。<br>

TKCH_VisemeLipSyncSetup.cs を Assets フォルダの下に配置します。（Assets フォルダの下に Editor サブフォルダを作成しても構いません。）<br>
![Assets フォルダの下に配置](ls1.png "Assets フォルダの下に配置")

ヒエラルキーにモデルを配置し、VRC_AvatarDescriptorコンポーネントを追加したら、<br>
Inspectorウィンドウの歯車アイコンから VisemeLipSyncSetup を実行すると、<br>
「Face」、「Body」、その他の順で Lip Sync 対応オブジェクトを検索し、自動で設定します。<br>

![歯車アイコンから VisemeLipSyncSetup を実行](ls2.png "歯車アイコンから VisemeLipSyncSetup を実行")

動作確認した環境
- Unity 2017.4.28f1
- VRCSDK-2019.06.19.20.14

動作確認したモデル
- Mikoko （ https://nekomasu.wixsite.com/kemomimioukoku/mikoko ）
- Nekoma （ https://nekomasu.wixsite.com/kemomimioukoku/nekoma ）

## Author

github:[eijis](https://github.com/eijis-pan)  または twitter: @ eijis_pan

## Disclaimer

利用は自己責任でお願いします。<br>
本プログラムは、なんの欠陥もないという無制限の保証を行うものではありません。<br>
本プログラムに関する不具合修正や質問についてのお問い合わせもお受けできない場合があります。<br>
本プログラムの利用によって生じたあらゆる損害に対して、一切の責任を負いません。<br>
本プログラムの利用によって生じるいかなる問題についても、その責を負いません。
