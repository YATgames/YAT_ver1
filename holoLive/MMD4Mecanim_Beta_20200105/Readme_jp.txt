----------------------------------------------------------------------------------------------------------------------------------------------------------------

MMD4Mecanim 2020/01/05 版(β)

by Nora
http://stereoarts.jp

----------------------------------------------------------------------------------------------------------------------------------------------------------------

■ MMD4Mecanim

Unity で MMD の各種モデルデータ / モーションデータを扱うためのコンバータ及びスクリプト群です。

■ MMD のモデルデータ及びモーションデータを使用するにあたって

各種モデルデータ及びモーションデータに同封されている説明書等に記されている利用規約に必ず目を通し、
遵守していただけるようにお願いします。

配布を目的とするゲーム及びコンテンツでの利用について、利用規約で明示的に許可している場合を除いて、
作品内での使用及びデータを含んだ状態での配布を行って良いか、必ずモデルやモーションデータの制作者に確認をお願いします。
※多くのモデルやモーションデータについては、動画及び静止画での使用を前提としており、それ以外での使用を前提としていません。

クリプトン・フューチャー・メディア株式会社の権利を有するキャラクターを用いて創作活動を行う場合、
必ずガイドラインに目を通していただけるようにお願いします。

ピアプロ・キャラクター利用のガイドライン
http://piapro.jp/license/character_guideline

■ 本ソフトウェア（またはコンポーネント）の商用でのウェブや雑誌掲載について

本ソフトウェア（またはコンポーネント）の商用でのウェブや雑誌掲載について、事前に作者（Nora）へご相談頂けるようお願いします。
また、頻繁にバージョンアップすることから、バージョンの表記もお願いします。他、少しでも動作がおかしいと感じた場合は、お問い合わせください。

■ サポート対象のプラットホームについて

現在、Windows, Mac, WSA(UWP), iOS, Android での動作をサポートしています。
ただし、何かしらの問題が発生した際にはサポートの停止、またはソフトウェアの配布停止を行う場合があります。
もし問題のある使われ方を発見された際には、直接ご本人にご指摘されるか、サイトの通報・報告システムを利用するなどして対処にご協力頂けるよう、
何卒宜しくお願い致します。

■ Android サポートについての補足

公開バージョンでは、Unity から apk をビルドした際に接続していた端末でのみ動作するようにロックをしています。
この制限は、著作者・権利者に無断で Google Play にビルドをアップしたり、または Dropbox などでの共有を防ぐための措置です。
また、この制限を意図的に解除したり、そのノウハウを共有しないでください。
※もし、著作者及び権利者ご本人で、何らかの事情でこの制限を解除したい場合は、ご相談ください。

■ サポート対象外のプラットホームについて

サポート対象外のプラットホーム(WebPlayer, WebGL, その他)の動作について、配布バージョンでは動作しないよう設定してあります。
ハック的に動作させたり、そのノウハウの共有、ビルドや動画・静止画のアップロード、SNS投稿など、私的使用外での使用はしないでください。
※もし、著作者及び権利者ご本人で、何らかの事情でこれを必要とする場合は、ご相談ください。

■ 変換モデルとモーションの使用について

WebPlayer, WebGL などサポート対象外の環境では、私的使用を除き、使用しないでください。

その他、マテリアルや表情モーフ、モーションの再現性の低い環境では、私的使用を除き、使用しないでください。
（キャラクターイメージを損なう使用は避けてください。）

また、3Dプリント（立体出力）向けの使用も禁止します。

■ ツール及びソースコード再配布について

原則として、再配布を禁止とします。
オンラインストレージへのアップロードや、github のようなパブリックなバージョン管理サービスもこれに該当します。

■ オンラインへのアップロード、及び iTunes, Google Play などのストアアプリの公開について

商用・非商用問わず、他者の著作物（モデル・モーション・楽曲など）が含まれている場合は、原則として公開・配布しないでください。
※対象に含まれる全ての著作物の著作者及び権利者に対して、明示的に承諾を得る必要があります。

■ VR Chat について

VR Chat へのアップロード、及び使用は原則として行わないでください。
ほとんどのモデルは再配布（アップロード）が禁止されています。
もしアップロード済みのモデルがある場合は、削除をお願いします。

■ 改造・及び解析行為について

パッケージに含まれる EXE, DLL, その他ライブラリについて、一切の改造・解析（逆アセンブル等）は行わないでください。

■ 免責事項

本ソフトウェア（またはコンポーネント）を使用したことによる一切の損害（一次的、二次的に関わらず）に対し、こちらでは一切の責任を負いません。

■ 補足

上記制限は過去のバージョンにおいても同様に発生します。

----------------------------------------------------------------------------------------------------------------------------------------------------------------

■ 使用ライブラリ

Autodesk FBX SDK 2016.1
http://help.autodesk.com/view/FBX/2016/ENU/

Bullet Physics Library 2.75 (PMX2FBX / MMD4MecanimBulletPhysics)
Bullet Physics Library 2.83 (PMX2FBX / MMD4MecanimBulletPhysics)
Copyright (c) 2003-2006 Erwin Coumans  http://continuousphysics.com/Bullet/

bullet-xna
https://code.google.com/archive/p/bullet-xna/

MeCab 0.996(BSDを選択)
MeCab IPA辞書 2.7.0
http://taku910.github.io/mecab/

----------------------------------------------------------------------------------------------------------------------------------------------------------------

■ Licenses

FBX SDK 2016.1

This software contains Autodesk® FBX® code developed by Autodesk, Inc. Copyright 2015 Autodesk, Inc. All rights, reserved. Such code is provided “as is” and Autodesk, Inc. disclaims any and all warranties, whether express or implied, including without limitation the implied warranties of merchantability, fitness for a particular purpose or non-infringement of third party rights. In no event shall Autodesk, Inc. be liable for any direct, indirect, incidental, special, exemplary, or consequential damages (including, but not limited to, procurement of substitute goods or services; loss of use, data, or profits; or business interruption) however caused and on any theory of liability, whether in contract, strict liability, or tort (including negligence or otherwise) arising in any way out of such code.”

See more.
http://help.autodesk.com/view/FBX/2016/ENU/



Bullet Physics

This software is provided 'as-is', without any express or implied warranty.
In no event will the authors be held liable for any damages arising from the use of this software.
Permission is granted to anyone to use this software for any purpose,
including commercial applications, and to alter it and redistribute it freely,
subject to the following restrictions:

1. The origin of this software must not be misrepresented; you must not claim that you wrote the original software. If you use this software in a product, an acknowledgment in the product documentation would be appreciated but is not required.
2. Altered source versions must be plainly marked as such, and must not be misrepresented as being the original software.
3. This notice may not be removed or altered from any source distribution.

See more.
http://bulletphysics.org



MeCab 0.996

Copyright (c) 2001-2008, Taku Kudo
Copyright (c) 2004-2008, Nippon Telegraph and Telephone Corporation
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are
permitted provided that the following conditions are met:

 * Redistributions of source code must retain the above
   copyright notice, this list of conditions and the
   following disclaimer.

 * Redistributions in binary form must reproduce the above
   copyright notice, this list of conditions and the
   following disclaimer in the documentation and/or other
   materials provided with the distribution.

 * Neither the name of the Nippon Telegraph and Telegraph Corporation
   nor the names of its contributors may be used to endorse or
   promote products derived from this software without specific
   prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED
WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR
TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

See more.
http://taku910.github.io/mecab/

----------------------------------------------------------------------------------------------------------------------------------------------------------------

■ 履歴
2019/01/05 Unity 2018.4 以降で AnimationClip がフルパスになっているのを自動補正するスクリプトの再修正
2019/10/15 Unity 2018.4 以降で AnimationClip がフルパスになっているのを自動補正するスクリプト追加
2018/05/23 Unity 2018.1 (正式版) で一部エラーが出ていたのを修正
2018/03/04 下記参照
- [MMD4Mecanim] Unity 2018.1 への対応
- [MMD4Mecanim] iOS への bitcode 対応が不完全だったのを修正
2017/09/10 [MMD4Mecanim] ランタイムコードを少しだけ最適化
2017/09/09 Android サポートの限定的な解除と, それに伴い規約更新
2017/09/04 下記参照
- [MMD4Mecanim] Unity 2017.2 への対応
- [MMD4Mecanim] WSA(UWP) 及び iOS のサポートを追加, それに伴い規約更新
- [MMD4Mecanim] 一部の古い Unity バージョンでスフィアマップの設定に失敗していたのを修正
- [MMD4Mecanim] 初回インポート時に各種DLLやライブラリのインポート設定が崩れる現象があったのを修正
- [MMD4Mecanim] x86 ビルドで起動に失敗していたのを修正
2017/06/29 一部モデルでマテリアルインポート時にエラーが発生していたのを修正
2017/06/04 PMX2FBX の規約にも一部追記
2017/06/02 規約を一部追記, VRChatに関する注記及びロック機能の追加
2017/04/23 下記参照
- [MMD4Mecanim] Unity 5.6 / 2017.1 への対応
- [MMD4Mecanim] モデルのプロパティ変更時、Dirtyをマークするように修正
- [MMD4Mecanim] マテリアルページの表示が異常に重くなる場合があったのを修正
- [MMD4Mecanim] 初回インポート時にスフィアのマテリアル設定に失敗していたのを修正
2017/01/28 規約文面のみ修正
2016/12/06 プラグイン類を再ビルド(内部調整)
2016/12/04 プラットホーム切り替え時の動作を変更
2016/11/06 下記参照
- [Shader] Standard の加算スペキュラーの Inspector 対応
- [Shader] Standard シェーダーの一部機能を 5.4 相当にアップデート
2016/11/05(2) [Shader] Standard の加算スペキュラー対応(実験的)
2016/11/05 [MMD4Mecanim] テッセレーションの適用比率の動的変更に対応
2016/11/03 [PMX2FBX] エッジのカスタム伸長機能を追加, ほぼ同じ座標の頂点の座標を完全一致させる機能を追加(どちらも標準では無効)
2016/11/01 下記参照
- [MMD4Mecanim] UIの表示状態をある程度保存するように調整, PMX2FBX が同封されていない場合に変換項目を表示しないように調整
2016/10/29 下記参照
- [Shader] テッセレーションの対応
- [Shader] いくつかのプロパティ(ToonTone, ShadowLum, テッセレーション関連)を個別設定できるように UI 追加
- [PMX2FBX] SplitMeshBone のバグ修正と最適化（モーフ影響下のメッシュ分離時、エラーが出力される可能性があったのを修正）
2016/10/28 下記参照
- [MMD4MecanimBulletPhysics] 剛体パラメーターのカスタマイズ対応
- [PMX2FBX] SplitMeshBone の各種バグ修正
2016/10/26(2) SplitMeshBone の各種バグ修正
2016/10/26 下記参照
- [MMD4MecanimBulletPhysics] ScriptExecutionOrderの初期値を20001に、任意の値を再設定できるように修正
- [PMX2FBX] Advancedにボーン名によるメッシュ分離(SplitMeshBone)の追加
2016/10/13 下記参照
- [PMX2FBX] NoShadowCasting かつ ShadowReceiving するマテリアルのメッシュを分離、標準で有効化
- [PMX2FBX] Joint(Constraint)のAdditionalLimitAngle対応
- [Shader] ToonTone (階調) のカスタマイズ対応
- [MMD4Mecanim] NoShadowCasting かつ ShadowReceiving するマテリアルを持つ Renderer の castShadowing を強制オフ
- [MMD4MecanimBulletPhysics] IKのウェイト値が正しく反映されていなかったのを修正
- [MMD4MecanimBulletPhysics] Joint(Constraint)のAdditionalLimitAngle対応
- [MMD4MecanimBulletPhysics] マルチスレッドで, ワールドを完全同期するオプション(Synchronize)を追加
2016/09/04 Unity 5.5 でクリップ名にピリオドが入っているとアニメーションステートの生成に失敗していたのを修正
2016/09/01 起動時の環境チェックの微修正, Standard を微修正(BRDF1)
2016/08/31 Unity 5.5 に暫定対応, Unity 5.4 で Shader の警告が出ないように修正
2016/08/15 [Shader] Standard で Shader Model 2.0 向けビルドが失敗するケースがまだあったので一時的に無効化
2016/08/11 [Shader] Offset 調整, AL を標準で無効化, スフィアテクスチャ無効時の補正調整
2016/08/09 下記参照
- [Shader] Standard のビルドバイナリで初期化に失敗しているケースがあったのを修正
- [Shader] Standard で Shader Model 2.0 向けのビルドが失敗していたのを修正
2016/08/06 [Shader] Standard で影のトーンが浅い不具合があったのを修正(内積計算の修正)
2016/08/05 [Shader] トゥーンの影の境界で、特定の条件によって荒が出ていたのを修正
2016/08/05 [Shader] Standard (透過)のシェーダーオフセットが正しく反映されていなかったのを修正
2016/08/04 下記参照
- [Shader] Standard のソース整理、BRDF 依存部分を別ソースに分離
- [Shader] Ambient Rate の廃止, Ambient To Diffuse を標準でオフ
- [Shader] Compatible 側のバリアント整理, 無駄なコードを生成しないように修正
- [Shader] 加算ライトが強く影響し過ぎていたのを修正
2016/08/03 [Shader] Standard で BRDF1 が選択された場合にトゥーン色が正しく反映されない可能性がったのを修正
2016/08/01 下記参照
- [Shader] エッジ、半透明のオフセットの扱いに不整合があったのを修正
- [Shader] Standard で ForceNoShadowCasting が正常に動作していなかったのを修正
- [Shader] エッジシェーダーのコード整理、最適化
- [Shader] マテリアルページで一部正常に機能していなかったのを修正
2016/07/31 下記参照
- [MMD4Mecanim] Standard Toon シェーダーを試験実装（5.3 以降, 別パッケージで用意）
- [MMD4Mecanim] Material ページを作り直し
- [MMD4Mecanim] SDEF 有効時に処理例外で落ちる場合があったのを修正
2016/07/18 Unity5の特定の古いバージョンでの不具合修正
2016/07/17 シェーダーの記述微修正、アニメーション圧縮を自動的に Optimal にするように調整, Unity4サポートの復旧
2016/07/14 スクリプトの実行順序の制御が機能していなかったのを修正
2016/07/12 IK処理を微調整, PMD向けの補正
2016/07/11 IK処理を微修正
2016/07/10 下記参照
- [MMD4Mecanim] 大部分のコードを Managed DLL に移行, Unity 4 を非サポート(過去バージョンからの更新は少し難しいため、新規プロジェクトでの使用を推奨)
- [PMX2FBX, MMD4MecanimBulletPhysics] 互換性向上のため、Bullet Physics の標準の動作バージョンを 2.75 に設定
- [PMX2FBX, MMD4MecanimBulletPhysics] Bullet Physics を 2.83 に更新、Bullet Physics の動作バージョンの変更に対応(2.75, 2.83)
- [PMX2FBX, MMD4MecanimBulletPhysics] IK処理修正、特定条件下でガクついていたのを修正
- [PMX2FBX] FBX SDK 2016.1 に更新
- [PMX2FBX] FBX の出力フォーマットで binary に対応（標準では ascii）
- [PMX2FBX] 出力時の回転順番を, 標準で XYZ から ZXY に変更
- [MMD4MecanimImporter] Unity Editor が 2D モード時にテクスチャインポートに失敗していたのを修正
- [MMD4Mecanim] 音声同期、音声ループの動作修正
- [MMD4MecanimBulletPhysics] シーンリロード時に動作が重くなるのを修正
- [MMD4MecanimBulletPhysics] シーンリロード時に物理の荒れが発生していたのを修正
2015/08/21 制限事項の追記, 未使用コードを除外, 英語の Readme を用意
2015/08/18 [MMD4MecanimBulletPhysics] Android / iOS のネイティブプラグインサポートを一旦オミット
2015/08/13 ソースに調整用メモ追加
2015/08/08 WebPlayer 及び WebGL プラットホームについて動作制限を追加
2015/06/19 Unity5 & iOSでのビルドエラー修正
2015/06/07 チュートリアルに利用規約に関する補足追記
2015/05/08 [MMD4MecanimImporter] 利用規約チェック機能の強化
2015/05/03 [MMD4MecanimImporter] 利用規約の確認画面の文言調整、ガイドラインへのリンクボタン追加
2015/04/28 [MMD4MecanimBulletPhysics] iOS のエディターモードでプラグインが動作していなかったのを修正
2015/04/28 下記参照
- [MMD4MecanimBulletPhysics] Android / iOS のネイティブプラグインのサポート
2015/04/25 下記参照
- [PMX2FBX] 軸制限IKの動作改善
- [MMD4MecanimBulletPhysics] ボーンの変形階層のリファクタリング(PMX2FBX準拠)
- [MMD4MecanimBulletPhysics] IKのリファクタリング(PMX2FBX準拠)
- [MMD4MecanimBulletPhysics] 最適化, マルチスレッドでも必要最小限のボーンしか書き戻さないように修正
- [MMD4MecanimBulletPhysics] レガシーのBlendShapesを使わない表情モーフが動作していなかったのを修正
- [MMD4MecanimBulletPhysics] ローカル軸のサポート
- [MMD4MecanimBulletPhysics] Vsyncオフの場合の動作改善, 物理のワールド未更新でも荒れを出にくいように調整
- [MMD4MecanimBulletPhysics] 物理のフレームレートが低い場合でも荒れを出にくいように調整
- [MMD4MecanimBulletPhysics] C#版を, ネイティブ版の変更を全てフィードバック
- [MMD4MecanimBulletPhysics] C#版でスレッドのスパイクが起きていたのを修正
- [MMD4MecanimBulletPhysics] paralellDispacher / paralellSolver のオプション追加
2015/04/18 下記参照
- [PMX2FBX] ボーン名関連のオプション名変更(Rename -> BoneRename, PrefixRename, PrefixBoneRename)
- [PMX2FBX] モーフ名のプリフィクス除外に対応
- [PMX2FBX] マテリアル名の日本語名出力＆プリフィクス除外に対応
2015/04/17(2) 下記参照
- [Shader] スペキュラで影の部分の描画がおかしくなっていたのを修正
- [PMX2FBX] ルートの RenameFlag 無効時の名前を調整
- [PMX2FBX] Prefix 系のリネームも外部で抑止できるように調整
- [MMD4MecanimBulletPhysics] 暫定的にマルチスレッド動作時のルートトランスフォームへの補正を無効化
2015/04/17 下記参照
- [PMX2FBX] PMD/PMXのボーンの変形順序/FK/IK/付与のリファクタリング
- [PMX2FBX] PMDで相互に回転影響下/回転連動を指定した場合の補正処理追加
- [PMX2FBX] Quaternion の Slerp の動作改善, 一部の補間で若干誤差が出ていたのを修正
- [PMX2FBX] 捩りボーン/軸制限の VMD 再生時の補正対応(PMX2FBX > FixedAxis)
- [PMX2FBX] 捩りボーン/軸制限の IK 処理を追加(PMX2FBX > IKFixedAxis)
- [PMX2FBX] ローカル軸に限定的に対応(PMX2FBX > LocalAxis)
- [PMX2FBX] デバッグ用に全てのフレームを出力する機能を追加(PMX2FBX > AnimKeyReduction)
2015/04/05 下記参照
- [MMD4MecanimAnimMorphHelper] float 誤差修正
- [MMD4MecanimModel] マテリアル初期化時にNull Referenceが発生する可能性があったのを修正
2015/03/28 下記参照
- [Shader] AmbientRate のサポート(Unity5 の Skybox 向け)
- [MMD4MecanimModel] インスタンス消失時の復旧処理追加
2015/03/18 [MMD4MecanimModel] Delayed Awake Frame でオーディオ未設定時にアニメーション停止していたのを修正
2015/03/17 下記参照
- [MMD4MecanimModel] アニメーション同期の動作改善, Delayed Awake Frame (初期化待機フレーム数) のサポート
- [PMX2FBX] 2012/03/08 以前のレガシーバージョンを別パッケージで同封
2015/03/12 下記参照
- [Shader] Unity4/5でビルドエラーが出ていたのを修正
- [PMX2FBX] enableFBXTexture のサポート(PMX2FBX > Advanced)
- [PMX2FBX] PMXのFK/IK/付与のリファクタリング, IK影響下+変形階層使用時の動作改善
2015/03/08(2) [Shader] Ambient 周りを再調整
2015/03/08 下記参照
- [MMD4MecanimModel] Unity5 でのコンパイルエラー・警告類の排除
- [MMD4MecanimBulletPhysics] Unity5 でネイティブプラグインが Personal で解禁されたので使用できるように調整
- [Shader] Unity5 でライト演算の差異(明るすぎる問題)の対処, Unity4 と共存させるために各種調整
- [Shader] Unity5 の IBL 向けに AmbientToDiffuse のサポート
- [Shader] Deferred(Legacy) のサポートの廃止
2015/02/14 下記参照
- [MMD4MecanimModel] アニメーションのステート切り替え時に、モーフが更新されない場合があったのを修正
- [UnityChanMorph] ２体以上に設定した場合に正しく反映されなかったのを修正
2014/12/31 下記参照
- [PMX2FBX] UTF-16/Null文字で終わる文字列のパースに失敗していたのを修正
- [MMD4MecanimImporter] Reset Humanoid Mappingで、WriteAllText実行前にDeleteを実行するように修正
- [MMD4MecanimModel] Morphページの表示でNull参照が起きる場合があったのを修正
2014/12/27 下記参照
- [PMX2FBX] IKLoopMinIterations(最少ループ回数)の追加(標準値 : 8)
- [PMX2FBX] モーフ名の英訳へのリネーム追加(Advanced>Global Settings>Advanced>MorphRename)
- [MMD4MecanimModel] GetMorph() の英訳へリネームされたモーフ名への追加対応
2014/12/13 下記参照
- [MMD4MecanimBulletPhysics] dll を x86 フォルダに移動
- [ドキュメント] MMD4Mecanim リアルタイム版 Bullet Physics を更新
2014/12/08 下記参照
- [PMX2FBX] IK後の親子階層の変形が正しく行われていなかったのを修正
- [MMD4MecanimBulletPhysics] 既に使われなくなったソースコードを除外
- [ドキュメント] MMD4Mecanim チュートリアル（応用編）を更新
2014/12/01 下記参照
- [Shader] スペキュラーの描画不具合再調整
- [Shader] NEXTEdgeシェーダーのビルドで一部環境でエラーが発生していたのを修正
- [Material] スペキュラーの処理フラグを個別で有効・無効を切り替えられるように調整
2014/11/27 [Shader] ShaderKeyword による最適化を自動適用(スペキュラの不具合修正も含む)
2014/11/23 下記参照
- [Shader] ShadowCaster / ShadowCollecter でアルファ値のクリップ処理を有効
- [MMD4MecanimBulletPhysics] マルチスレッド処理を標準で有効化
- [MMD4MecanimBulletPhysics] IK処理リファクタリング, AdditionalColliderを標準でオフ
- [PMX2FBX] IK処理リファクタリング, AdditionalColliderを標準でオフ
2014/11/18 下記参照
- [Shader] アルファ値のクリップ値を再調整, エッジに対してもクリップ値を有効化
- [MMD4MecanimImporter] マテリアルのシェーダー判定で、マテリアルモーフを持つ場合は強制的に透過を使用
- [MMD4MecanimModel] マテリアルモーフの強制無効化に対応(標準では有効)
2014/11/17 下記参照
- [MMD4MecanimImporter] シェーダー最適化のために ShaderKeyword の各種対応
- [Shader] 最適化, Specular/Emissive/Sphere/SelfShadow処理をShaderKeywordで制御するようにした
- [Shader] アルファ値がほぼ完全に 0 の場合のみ, Ｚ書き込みをスキップするように修正
2014/11/16 下記参照
- [Shader] スフィア(加算)の再現性向上
- [Shader] 完全透明でもＺ書き込みするように修正
- [MMD4MecanimImporter] TGAの透過判定のバグ修正(ファイルヘッダの解析部分を微調整)
- [MMD4MecanimBulletPhysics] インターフェイス調整中(内部的な引数調整中)
2014/11/01 下記参照
- Unity 5.0.0 beta9 仮対応(エディターモードでのみ動作。Unity 5 の仕様変更により, 変化する可能性があります。)
- [Shader] renderQueue の標準値調整 & セルフシャドウが無効になっていたのを修正
- [MMD4MecanimImporter] PrefixRenderQueue & RenderQueueAfterSkybox 追加(renderQueueの事前補正)
- [MMD4MecanimModel] RenderQueueAfterSkybox 追加(PostfixRenderQueue の補正値を Skybox より後ろに補正)
- [NEXTEdge] RenderQueue 補正を PostfixRenderQueue の仕様変更に伴い調整
2014/10/20 下記参照
- [MMD4MecanimModel] 書き換え対象のメッシュに MarkDynamic() 設定
- [MMD4MecanimBulletPhysics] MMD4MecanimAgent.exe を廃止
- [PMX2FBX] 標準で AnimNullAnimation / AnimRootTransform をオフに変更(レガシーの排除)
2014/10/18 MMD4MecanimModelのRigidBody無効フラグが毎回リセットされていたのを修正
2014/10/02 使用DLLからkakasiを除外, MeCabに変更
※ PMX2FBX の旧ビルド向けの移行パッケージは別途用意
2014/09/29 PCLライセンスについて、注釈をライセンス確認画面に追加
2014/09/27 [Shader] 半透明シェーダーを Skybox よりも描画順が後になるように調整
2014/09/24 下記参照
- [PMX2FBX] 表示制御のみでIK情報のないVMDモーションが変換できなかったのを修正
- [MMD4MecanimBulletPhysics] 2.75 ビルドで 09/15 の変更がフィードバックされていなかったので, 最新に差し替え
2014/09/18 [Shader] NEXT Edge 対応
2014/09/15 下記参照
- [MMD4MecanimBulletPhysics] 初期位置が原点より大きく移動する場合, 物理が破綻する可能性があったのを修正
- [MMD4MecanimBulletPhysics] グローバルワールドの, 重力関係のプロパティのリアルタイムフィードバックに対応
- [MMD4MecanimBulletPhysics] gravityNoise に対応
- [MMD4MecanimModel] エディターでマテリアルを個別に調整した場合、正しく反映するように調整
- [MMD4MecanimModel] デバッグ用コリジョン生成の調整, マージン対応
2014/09/11 下記参照
- [MMD4MecanimBulletPhysics] IK/物理設定を持たないモデルを読み込んだ場合に発生していたエラーの修正
- [PMX2FBX] 中指ボーンが"Fingers"になっていたので"Middle"に修正
2014/09/07 [MMD4MecanimBulletPhysics] Final IK より処理順番が後になるように調整
2014/09/06 下記参照
- [PMX2FBX] XMLの変換が異常に遅くなっていたのを修正
- [PMX2FBX] 物理の床配置に対応(標準でオン)
- [PMX2FBX] VMDのIKオンオフ、物理オンオフに対応
- [MMD4MecanimBulletPhysics] BlendShapes無効時、グループモーフが誤作動していたのを修正
- [MMD4MecanimBulletPhysics] 物理の床配置に対応(標準でオフ)
2014/08/30 下記参照
- PMX2FBX と MMD4MecanimBulletPhysics の処理を一部共通化
- Bullet Physics 2.75 の btQuaternion::slerp, btAsin, btAcos のバグ修正
- [PMX2FBX] Autodesk FBX SDK を 2015.1 に変更(Windows / Mac 両対応)
- [PMX2FBX] プロジェクト構成を再構築, Bullet Physics のバージョン切り替えに対応
- [PMX2FBX] Bullet Physics を 2.82 に変更
- [PMX2FBX] Bullet Physics 2.75 ビルドを同封(Windows / Mac 両対応)
- [MMD4MecanimBulletPhysics] プロジェクト構成を再構築, Mac でも Bullet Physics のバージョン切り替えに対応
- [MMD4MecanimBulletPhysics] (Windows) 64 ビット版のプラグインを同封
- [MMD4MecanimBulletPhysics] Bullet Physics を 2.82 に変更
- [MMD4MecanimBulletPhysics] Bullet Physics 2.75 ビルドを同封(Windows / Mac 両対応)
- [MMD4MecanimBulletPhysics] (BulletXNA) 回転補間で稀に NaN を返す可能性がったのを修正
- [PMX2FBX] 物理後変形で一部機能しないモデルがあったのを修正
- [MMD4MecanimModel] UserRotation が正しく機能していなかったのを修正
- [MMD4MecanimBulletPhysics] WebPlayer で一部 Standalone 用の処理が入っていたのを削除
2014/08/21 下記参照
- [PMX2FBX] ジョイントから参照している剛体が存在していなくても処理を続行するように修正
2014/08/19 下記参照
- [MMD4MecanimBulletPhysics] (BulletXNA) 剛体を配置した場合に Null Reference が起きていた問題の修正
- [MMD4MecanimModel] エディターモードでモーフ変化させた場合, 設定値がリセットされていた問題の修正
2014/08/10 下記参照
- [MMD4MecanimImporter] FBX再インポートチェックを最小限にするように調整
- [MMD4MecanimImporter] FBX再インポート時に初回以外はマテリアルを初期化しないように調整
- [MMD4MecanimImporter] FBX再インポート時に複数回マテリアルを初期化していたのを修正
- [MMD4MecanimImporter] コンパイル後, または再生停止後に状態チェックをしないように調整
- [MMD4MecanimModel] 初回実行時に MMD4MecanimModel が自動的に追加されない不具合の再修正
- [MMD4MecanimModel] アニメーション関連のリファクタリング, 処理の共通化, 音声同期の若干の改善
2014/08/05 下記参照
- パッケージのビルドを 4.5.2f1 ベースに変更
- 変換用 Inspector の UI を基本モードと拡張モードに分離
- [MMD4MecanimModel] 初回実行時に MMD4MecanimModel が自動的に追加されない不具合の修正
- [MMD4MecanimModel] SDEF変形をネイティブプラグインに移植, マルチスレッド化
- [MMD4MecanimModel] 非 BlendShapes での頂点モーフをネイティブプラグインに移植, マルチスレッド化
- [MMD4MecanimBulletPhysics] マルチスレッドの動作を改善, CPU負荷低減( Sleep(0) を行わないようにした )
- [MMD4MecanimBulletPhysics] メッシュの頂点更新の高速化のために timeBeginPeriod(1) を使用 ( Windows 限定, 必要な場合のみ )
- [MMD4MecanimBulletPhysics] (BulletXNA) メッシュの頂点更新の高速化のために timeBeginPeriod(1) を実行するツールを追加( Windows 限定 )
- [MMD4MecanimBulletPhysics] (BulletXNA) ワールド更新の同時実行数を 1 に制限( 複数同時に実行しようとすると落ちるため )
- [PMX2FBX] BlendShapesを標準で有効化
- [PMX2FBX] 頂点モーフのメッシュ分離で, 同一マテリアルも結合するオプション追加(DrawCall削減, 描画高速化)
- [PMX2FBX] SDEF/QDEFの頂点分離を標準で無効化
2014/07/30 [MMD4MecanimBulletPhysics] Windows/MacOSX 以外のプラットホームでコンパイルエラーが出ていたのを修正
2014/07/29 下記参照
- [MMD4MecanimModel] IK/ボーン付与/Humanoid肩ボーン補正をMMD4MecanimBulletPhysicsに移植
- [MMD4MecanimModel] リアルタイムでのボーンモーフのサポート(ボーン処理の移植に伴いMMD4MecanimBulletPhysics側で動作)
- [MMD4MecanimBulletPhysics] FBX の Import Scale を変更した場合, 再変換しなくてもスケール値をフィードバックできるように調整
- [MMD4MecanimBulletPhysics] モデルのスケール値を(1,1,1)以外にした場合, 剛体の干渉がうまくいってない部分があったのを修正
- [MMD4MecanimBulletPhysics] マルチスレッドのサポート(標準ではオフ)
- [MMD4MecanimBulletPhysics] 物理後付与/IKを正しく動作するように調整
- [MMD4MecanimBulletPhysics] BulletXNA 版にも各種変更をフィードバック
- [MMD4MecanimBulletPhysics] 2.75 版も各種変更をフィードバック
- [PMX2FBX] DisableRigidBody を FreezeRigidBody にリネーム
2014/07/17 下記参照
- UnityChanMorph を追加
- MMD4MecanimUnityChanMorphHelper を追加
2014/06/28 下記参照
- [MMD4MecanimBulletPhysics] (Windows) DLL の場所を Plugins/ に戻す
- [MMD4MecanimBulletPhysics] リアルタイムでの重力パラメーター変更に対応
- [MMD4MecanimBulletPhysics] リアルタイムでの剛体オンオフに対応
- [MMD4MecanimModel] IKリファクタリング, 足/膝/足首それぞれの補正を有効化
- [MMD4MecanimModel] BoneInherenceEnabled を標準で無効化(CPU負荷低減)
- [PMX2FBX] IKリファクタリング, 足/膝/足首それぞれの補正を有効化
- [PMX2FBX] 物理無効オプションが機能していなかったのを修正
- [PMX2FBX] キーフレームリダクションの回転値に対する許容値を変更(精度向上)
2014/06/22 下記参照
- [MMD4MecanimBulletPhysics] (Windows) DLL の場所を Plugins/x86 に移動
- [MMD4MecanimBulletPhysics] Bullet Physics 2.75 でビルドしたバージョンを同封
- [MMD4MecanimBulletPhysics] Bullet Physics 2.79 ベースで可能な限り 2.75 に近づけるように再構築
- [MMD4MecanimBulletPhysics] 角速度フィルタ, 突き抜け防止コリジョン判定, CCD有効化, ジッター除去フィルタ
- [MMD4MecanimBulletPhysics] 更新ステップを厳密化(Accurate Step)
- [MMD4MecanimModel] IK/回転付与の厳密化
- [MMD4MecanimModel] エディターでシェーダーのライトプロパティが余計に更新されないように修正
- [BulletXNA-Unity] DLL の場所を BulletXNA に移動
- [BulletXNA-Unity] Bullet Physics 2.79 ベースで可能な限り 2.75 に近づけるように再構築
- [Shader] エッジシェーダーの Diffuse ライト調整, Offset でメッシュとの重なりを低減
- [PMX2FBX] Bullet Physics 2.79 ベースで可能な限り 2.75 に近づけるように再構築
- [PMX2FBX] 角速度フィルタ, 突き抜け防止コリジョン判定, CCD有効化, ジッター除去フィルタ
- [PMX2FBX] IK/回転付与の厳密化
2014/05/24 下記参照
- [PMX2FBX] ボーン位置合わせの再現性向上
- [MMD4MecanimBulletPhysics] ボーン位置合わせの再現性向上
- [MMD4MecanimBulletPhysics] Bullet Physics 2.79 から 2.75 に変更(Windows用DLLのみ)
2014/05/21(2) - [MMD4MecanimModel] Unity用のコライダーを生成する機能を暫定で追加
2014/05/21 下記参照
- [MMD4MecanimImporter] モデル変換時、マテリアルを上書きしないように調整
- [MMD4MecanimImporter] マテリアルの自動設定を個別にオンオフできるように機能追加
2014/05/18(2) [MMD4MecanimModel] シェーダー補正が MMD4Mecanim 標準シェーダー以外でも有効化してしまっていたのを修正
2014/05/18 下記参照
- [資料] チュートリアル(基本編、応用編)をそれぞれ最新版で書き直し
- [MMD4MecanimImporter] エディタGUIのフォントサイズが大きいままになってしまう不具合の修正
- [MMD4MecanimImporter] モデルの利用規約表示にファイルに埋め込まれた名前とコメントも表示する機能を追加
2014/05/14 下記参照
- [Shader] レガシーの機能を排除(LambertStr/AddLambertStr)
- [Shader] スフィアの座標計算を厳密にするように変更(tex2DからtexCUBEで求めるように変更)
- [Shader] スフィアテクスチャの指定を Texture2D から CubemapTexture へ変更(厳密化の影響)
- [Shader] ForwardAdd(追加ライト)で背面も明るくなっていた不具合の修正(AddLightToonCen / AddLightToonMinの追加)
- [Shader] Deferred Lightingで追加ライト部分が白っぽくなっていた不具合の修正
- [Shader] Deferred Lightingでライト色 x ライト強度の乗算をマテリアル設定時に行うように修正
- [Shader] エッジサイズのスケール補正をシェーダーではなくマテリアル設定時に行うように修正
- [Shader] スペキュラーのスケール補正をシェーダーではなくマテリアル設定時に行うように修正
- [Shader] シェーダー内部の変数を一部事前計算するように修正(TempDiffuse/TempAmbientといった変更の少ないもの)
- [Shader] Revision変数の追加
- [MMD4MecanimImporter] スフィア(加算)が存在しなかった場合の予備処理の追加(自動的に無効化する)
- [MMD4MecanimImporter] 今回のシェーダーの引数仕様変更に伴う自動チェック処理の追加
- [MMD4MecanimImporter] モデルの利用規約(Readme)を明示的に表示・確認する機能を追加
2014/05/09 [Shader] ForwardAddパスでライト色が白のみになっていた不具合修正
2014/05/08 下記参照
- [Shader] AutoLuminousの初期強度調整(1.0 -> 5.0)
- [Shader] SphereMapの最適化
- [MMD4MecanimImporter] SphereMapの自動再設定機能(新形式への自動対応)
- [MMD4MecanimMorph] AutoLuminousのLightUp / LightOffサポート
2014/05/07(2) [Shader] AutoLuminousを限定的にサポート(単純な発光のみ)
2014/05/07 下記参照
- [Shader] Deferred Lightingを作り直し, SA2Cの廃止, Blendの限定的サポート
- [Shader] Deferred Lightingで追加ライトにもトゥーンを適用するように調整
2014/05/02 下記参照
- [MMD4MecanimModelIK] リアルタイム版IKの再調整, ひざ関節の安定化
2014/04/30(3) 下記参照
- [MMD4MecanimModel] XDEF Enabled(SDEF変形)を標準ではオフに変更
2014/04/30(2) 下記参照
- [MMD4MecanimAnimMorphHelper] モーフアニメーションのキーフレーム補間で一部不具合があったのを修正
2014/04/30 下記参照
- [MMD4MecanimModelIK] 屈伸でジンバルロックが発生していた場合があったのを暫定対処
- [MMD4MecanimModelIK] 足首専用の角度補正追加
- [MMD4MecanimModel] モーフアニメーションのキーフレーム補間で一部不具合があったのを修正
2014/04/29 下記参照
- [MMD4MecanimModel] SDEFのサポート(C#, CPU)
- [MMD4MecanimModel] XDEF(SDEF/QDEF)の処理有効フラグの追加
- [MMD4MecanimModel] VertexDataの読み込み対応
- [MMD4MecanimData] 一部(IndexData)を MMD4MecanimAuxData に分離
- [MMD4MecanimImporter] .vertex.bytes形式のサポート(頂点の追加情報格納)
- [PMX2FBX] SDEF/QDEF頂点をスプリットする機能を追加
- [PMX2FBX] .extra.bytes(頂点を含む拡張情報)のサポート
- [MMD4MecanimModelIK] PMDの膝ボーンのリアルタイムIKの挙動修正
- [PMX2FBX] Humanoid 用のスケルトン推定部分の挙動修正
2014/04/05 下記参照
- [Shader] エッジのセルフシャドウ無効化(遮蔽物の影でエッジ部分が黒くなる現象の修正)
- [MMD4MecanimBone] boneInherenceEnabledGeneric を追加(標準値 false, Generic では標準で BoneInherence を無効化)
- [MMD4MecanimBone] pphEnabled を追加(標値 true, 強制的に有効になっていたのを, フラグ制御できるようにした)
- パッケージをUnity 4.3.3f1で作成するようにした
2014/04/04 下記参照
- [PMX2FBX] ～Bone4Mecanimの動作変更
- [PMX2FBX] モーフ頂点が(0,0,0)に固定されているものについては、SplitMesh の対象としないように調整
- [PMX2FBX] HumanTypeの判定対応
- [MMD4MecanimImporter] Rig設定ツールの追加
- PDF の利用規約を TXT にも追記, 掲載, 禁止事項について追記
2014/03/26 下記参照
- [MMD4MecanimModel] リアルタイムIKの最適化解除
- [MMD4MecanimBone] UserRotationの結合オーダー変更(localRotation * userRotation -> userRotation * localRotation)
※ Zigfu などで肩をあらかじめ水平にしたい場合への対処
2014/03/24 下記参照
- [PMX2FBX] Mecanim補正を強化, Mecanim(Humanoid)の適用可能なボーン構成を強化(テストビルド, Windowsのみ)
2014/03/21 [PMX2FBX] NullBoneAnimation は標準で無効化(メモリ不足対策)
2014/03/21 初版(以下はαからの変更点)
- [MMD4MecanimBone] リアルタイムIK / 回転付与の実装, リファクタリング
- [MMD4MecanimBone] リアルタイムIKでIKWeight/IKGoalを設定可能に
- [MMD4MecanimBone] リアルタイムIKの厳格化, 最適化
- [MMD4MecanimBone] リアルタイム回転付与の処理の厳格化, 通常モーションとうまく連動するように調整
- [MMD4MecanimModel] UpdateWhenOffscreenオプションの追加
- [PMX2FBX] NullBone / NullBoneAnimationを標準で有効化
