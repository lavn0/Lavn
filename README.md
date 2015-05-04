# Lavn
anything usefull for author

このプロジェクトは作者の作成物の保存用プロジェクトです。  
特に方向性はありません。  
有用なものが出来れば別プロジェクトに切り出します。  

## StyleCopSettingParser
StyleCopの設定ファイル("Settings.StyleCop")を管理しやすいように  
csvファイルからStyleCopファイルを作成する**コンソールアプリケーション**です。  
実行するとソリューションディレクトリ直下のStyleCopファイルを上書きします。

StyleCopファイルはデフォルト値から変更になった情報のみを保持しますが、  
このコンソールアプリケーションではデフォルト値を確認しないので、  
実運用上は

1. csvファイル上で設定のON/OFFを変更（TRUE/FALSEを変更）
2. StyleCopSettingParserでcsvからStyleCopファイルを生成
3. StyleCopファイルをStyleCop同梱のStyleCopSettingsEditorで保存（整形）

といった手順で使用します。  

