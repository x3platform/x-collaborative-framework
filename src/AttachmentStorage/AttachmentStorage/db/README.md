数据字典

**Table Name: 附件信息** 

| Column Name     | Type(Length)    | Default   | Description   | PK   | FK   | Index |  
| ----            | ----            | ----      | ----          | ---- | ---- | ----  |  
| Id              | varchar(36)     |
| EntityId        | varchar(50)     |
| EntityClassName | varchar(400)     |
| AttachmentName  | varchar(400)     |
    AttachmentName varchar(100) NULL ,
    VirtualPath varchar(1000) NULL ,
    FolderRule varchar(100) NULL ,
    FileType varchar(20) NULL ,
    FileSize int NULL ,
    FileStatus int NULL ,
    OrderId varchar(40) NULL ,
    CreateDate datetime NULL ,
    StorageNodeIndex int NULL 