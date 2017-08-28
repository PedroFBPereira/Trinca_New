
create login trinca with password = 'trinca', default_database=ChurrascosTrinca
use ChurrascosTrinca
exec sp_changedbowner @loginame='trinca'
