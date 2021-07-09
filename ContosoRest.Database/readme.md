#### To add a migration to the project
`Add-Migration -context AdminContext <migration_name>`

#### To add all missing migrations to the database
`Update-Database -context AdminContext`

#### To Remove the most recent migration from the project
`Remove-Migration -context AdminContext`

#### To Remove some migrations from a database
`Update-Database -context AdminContext <name_of_newest_migration_to_keep>`

#### To Remove all migrations from a database
`Update-Database -context AdminContext 0`