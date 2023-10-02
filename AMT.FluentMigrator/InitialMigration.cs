using FluentMigrator;
namespace AMT.FluentMigrator
{
    [Migration(20231001121400)]
    public class InitialMigration : Migration
    {
        public override void Down()
        {
            Delete.ForeignKey("FK_ChatMessage_From_User").OnTable("ChatMessage");
            Delete.ForeignKey("FK_ChatMessage_To_User").OnTable("ChatMessage");
            Delete.ForeignKey("FK_Password_HashingAlgorithm").OnTable("Password");
            Delete.ForeignKey("FK_Password_User").OnTable("Password");
            Delete.Table("ChatMessage");
            Delete.Table("Password");
            Delete.Table("HashingAlgorithm");
            Delete.Table("User");

        }

        public override void Up()
        {

            Create.Table("User")
                .WithColumn("Id").AsGuid().PrimaryKey()
                .WithColumn("Name").AsString(50).NotNullable()
                .WithColumn("LastName").AsString(50).NotNullable()
                .WithColumn("Username").AsString(100).NotNullable()
                .WithColumn("MFAEnabled").AsBoolean().NotNullable()
                .WithColumn("Verified").AsBoolean().NotNullable()
                .WithColumn("CreatedOnUtc").AsDateTime().NotNullable()
                .WithColumn("DeletedOnUtc").AsDateTime().Nullable()
                .WithColumn("IsDeleted").AsBoolean().Nullable(); ;

            Create.Table("ChatMessage")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("FromUserId").AsGuid().NotNullable()
                .WithColumn("ToUserId").AsGuid().Nullable()
                .WithColumn("Message").AsString(Int32.MaxValue).NotNullable()
                .WithColumn("HashedMessage").AsString(Int32.MaxValue).NotNullable()
                .WithColumn("CreatedOnUtc").AsDateTime().NotNullable()
                .WithColumn("DeletedOnUtc").AsDateTime().Nullable()
                .WithColumn("IsDeleted").AsBoolean().Nullable();

            Create.Table("Password")
                .WithColumn("Id").AsGuid().PrimaryKey()
                .WithColumn("UserId").AsGuid().NotNullable()
                .WithColumn("PasswordHash").AsString(Int32.MaxValue).NotNullable()
                .WithColumn("PasswordSalt").AsString(100).NotNullable()
                .WithColumn("HashAlgorithmId").AsInt32().NotNullable()
                .WithColumn("CreatedOnUtc").AsDateTime().NotNullable()
                .WithColumn("DeletedOnUtc").AsDateTime().Nullable()
                .WithColumn("IsDeleted").AsBoolean().Nullable();

            Create.Table("HashingAlgorithm")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("AlgorithmName").AsString(50).NotNullable()
                .WithColumn("Iterations").AsInt32().NotNullable()
                .WithColumn("CreatedOnUtc").AsDateTime().NotNullable()
                .WithColumn("DeletedOnUtc").AsDateTime().Nullable()
                .WithColumn("IsDeleted").AsBoolean().Nullable();

            Create.ForeignKey("FK_ChatMessage_From_User")
                .FromTable("ChatMessage").ForeignColumn("FromUserId")
                .ToTable("User").PrimaryColumn("Id");

            Create.ForeignKey("FK_ChatMessage_To_User")
                .FromTable("ChatMessage").ForeignColumn("ToUserId")
                .ToTable("User").PrimaryColumn("Id");

            Create.ForeignKey("FK_Password_HashingAlgorithm")
                .FromTable("Password").ForeignColumn("HashAlgorithmId")
                .ToTable("HashingAlgorithm").PrimaryColumn("Id");

            Create.ForeignKey("FK_Password_User")
                .FromTable("Password").ForeignColumn("UserId")
                .ToTable("User").PrimaryColumn("Id");
        }
    }
}