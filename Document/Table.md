## User (使用者)

| #   | Column  | Type          | Memo             |
| --- | ------- | ------------- | ---------------- |
| 1   | Id      | int           | autoNumber       |
| 2   | Name    | nvarchar(10)  | 暱稱             |
| 3   | Mail    | nvarchar(50)  | E-Mail(帳號名稱) |
| 4   | Pic     | nvarchar(200) | 大頭照圖檔路徑   |
| 5   | Guid    | varchar(36)   | Guid             |
| 6   | IsAdmin | bit           | 是否為管理者     |
| 7   | IsDel   | bit           | 是否停權         |

### Post (主要文章)

| #   | Column       | Type           | Memo       |
| --- | ------------ | -------------- | ---------- |
| 1   | Id           | int            | autoNumber |
| 2   | Content      | nvarchar(2000) | 文章內容   |
| 3   | IsDel        | bit            | 是否刪除   |
| 4   | CreateTime   | datetime       | 建立時間   |
| 5   | CreateUserId | int            | 建立人員   |
| 6   | UpdateTime   | datetime       | 更新時間   |
| 7   | UpdateUserId | int            | 更新人員   |

### Reply (回復訊息)

| #   | Column       | Type           | Memo         |
| --- | ------------ | -------------- | ------------ |
| 1   | Id           | int            | autoNumber   |
| 2   | PostId       | int            | 主要文章編號 |
| 3   | Content      | nvarchar(2000) | 回復內容     |
| 4   | IsDel        | bit            | 是否刪除     |
| 5   | CreateTime   | datetime       | 建立時間     |
| 6   | CreateUserId | int            | 建立人員     |
| 7   | UpdateTime   | datetime       | 更新時間     |
| 8   | UpdateUserId | int            | 更新人員     |

### Password (密碼)

| #   | Column     | Type     | Memo       |
| --- | ---------- | -------- | ---------- |
| 1   | Id         | int      | autoNumber |
| 2   | HashPw     | --       | 雜湊字串   |
| 3   | UserId     | int      | 使用者編號 |
| 4   | CreateTime | datetime | 建立時間   |
