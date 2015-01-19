数据字典
Data Dictionary  
  

**Table Name: tb_Attachment_File**  
_附件信息_

| 列名<br />Column Name      | 类型（长度）<br />Type(Length)    | 默认值<br />Default   | 描述<br />Description   | 主键<br />PK   | 外键<br />FK   | 索引<br />Index |  
| :--              | :--             | :--       | :---          | :--  | :--  | :--   |  
| Id | varchar(36) |  |  | √ |  |  
| EntityId | varchar(50) |  |  |  | √ |  
| EntityClassName | varchar(400) |  |  |  |  |  
| AttachmentName | varchar(100) |  |  |  |  |  
| SNI | int(11) |  | Storage Node Index |  |  |  
| VirtualPath | varchar(1000) |  |  |  |  |  
| FolderRule | varchar(100) |  |  |  |  |  
| FileType | varchar(20) |  |  |  |  |  
| FileSize | int(11) |  |  |  |  |  
| FileStatus | int(11) |  |  |  |  |  
| OrderId | varchar(40) |  |  |  |  |  
| CreateDate | datetime |  |  |  |  |  

**Table Name: tb_Attachment_DistributedFile**  

| 列名<br />Column Name      | 类型（长度）<br />Type(Length)    | 默认值<br />Default   | 描述<br />Description   | 主键<br />PK   | 外键<br />FK   | 索引<br />Index |  
| :--              | :--             | :--       | :---          | :--  | :--  | :--   |  
| Id | varchar(36) |  |  | √ |  |  
| VirtualPath | varchar(1000) |  |  |  |  |  
| FileData | blob |  |  |  |  |  
| CreateDate | datetime |  |  |  |  |  

**Table Name: tb_Attachment_Warn**  
_附件警告信息_

| 列名<br />Column Name      | 类型（长度）<br />Type(Length)    | 默认值<br />Default   | 描述<br />Description   | 主键<br />PK   | 外键<br />FK   | 索引<br />Index |  
| :--              | :--             | :--       | :---          | :--  | :--  | :--   |  
| Id | varchar(36) |  |  | √ |  |  
| WarnType | varchar(20) |  |  |  |  |  
| Message | varchar(200) |  |  |  |  |  
| AttachmentFileId | varchar(36) |  |  |  |  |  
| AttachmentName | varchar(100) |  |  |  |  |  
| VirtualPath | varchar(1000) |  |  |  |  |  
| FileType | varchar(20) |  |  |  |  |  
| FileSize | int(11) |  |  |  |  |  
| CreateDate | datetime |  |  |  |  |  
