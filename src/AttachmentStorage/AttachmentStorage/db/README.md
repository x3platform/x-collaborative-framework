数据字典

**Table Name: 附件信息** 

| Column Name      | Type(Length)    | Default   | Description   | PK   | FK   | Index |  
| :----            | :----           | :----     | :----         | :--- | :--- | :---- |  
| Id               | varchar(36)     |           |               |  √  |      |   √  |
| EntityId         | varchar(50)     |           |               |
| EntityClassName  | varchar(400)    |
| AttachmentName   | varchar(100)    |
| VirtualPath      | varchar(1000)   |
| FolderRule       | varchar(100)    |
| FileType         | varchar(20)     |
| FileSize         | int             |
| FileStatus       | int             |
| OrderId          | varchar(40)     |
| StorageNodeIndex | int             |
| CreateDate       | datetime        |
