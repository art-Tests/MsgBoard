## 更新紀錄

| 版號 | 說明         |
| ---- | ------------ |
| v2.0 | 專案分層     |
| v1.0 | 第一版完成品 |

## 專案說明

| #   | 專案                        | 說明                    |
| --- | --------------------------- | ----------------------- |
| 1   | Core/DataAccess/            | 存取資料庫              |
| 2   | Core/DataModel/             | 與資料庫相對應的 Entity |
| 3   | Core/HashUtility/           | 雜湊計算工具            |
| 4   | Web/MsgBoard/               | 網站主體                |
| 5   | Web.Lib/MsgBoard.BL/        | 網站使用之邏輯          |
| 6   | Web.Lib/MsgBoard.DataModel/ | 網站使用之 ViewModel    |

## 網站設定 (web.config)

| #   | 參數      | 說明                                                                     | 預設值        |
| --- | --------- | ------------------------------------------------------------------------ | ------------- |
| 1   | algList   | 雜湊工具使用的演算法，允許多組以逗號隔開，目前僅實作 `SHA512` , `SHA256` | SHA512,SHA256 |
| 2   | currectDb | 網站要使用的連線字串名稱，會依據設定找到該項 `connectionString` 的設定   | dbConnStr     |

連線字串預設值為

```
Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Art\Project\Personal\MsgBoard\MsgBoard\App_Data\Msg.mdf;Integrated Security=True
```

## 資料庫

1. Table Schema 的 Script 放置於 `Document` 目錄下

> 管理者並未實作介面控管，需要管理者權限需要手動於資料庫修改 `User.IsAdmin` 欄位為 `True`

## 前端

### typings 安裝

採用 `typeScript` 改寫過 `/Post/Index` 的前端 `javascript`

> 開發時期撰寫 typescript 會有不認識 jQuery 的情況，需要先安裝 typings，參考網站 [typings]

[typings]: https://www.npmjs.com/package/typings

```
npm install typings --global
typings install dt~jquery --global --save
```

### 編譯前端代碼

需先執行下列指令進行編譯的動作

```
npm install
npm run build:prod
```

## 已知問題

1. 修改會員若變更大頭照，未刪除舊圖檔
1. 修改密碼會留存舊有紀錄，未來需考量歷史資料刪除的問題
1. 表單錯誤提示是由後端給出，需改寫通過前端驗證才送至後端
1. 修改回覆後未提示該回覆的修改時間
