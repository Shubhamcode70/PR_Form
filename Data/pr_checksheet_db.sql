CREATE DATABASE IF NOT EXISTS pr_checksheet_db;
USE pr_checksheet_db;

CREATE TABLE IF NOT EXISTS PRNumberSequence (
  Id INT PRIMARY KEY,
  NextNumber BIGINT NOT NULL
);
INSERT INTO PRNumberSequence (Id, NextNumber)
VALUES (1, 0)
ON DUPLICATE KEY UPDATE NextNumber = NextNumber;

CREATE TABLE IF NOT EXISTS PurchaseRequisitionHeader (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    PRRequestId VARCHAR(50) NOT NULL UNIQUE,
    RequirementReceivedFrom VARCHAR(255) NOT NULL,
    DepartmentLocation VARCHAR(255) NOT NULL,
    PurposeOfProcurement TEXT NOT NULL,
    SingleVendorJustification TEXT NOT NULL,
    PRType VARCHAR(20) NOT NULL,
    CRID VARCHAR(100) NULL,
    AssetNumber VARCHAR(100) NULL,
    AttachmentPath VARCHAR(500) NULL,
    CreatedAt DATETIME NOT NULL DEFAULT UTC_TIMESTAMP()
);

CREATE TABLE IF NOT EXISTS PurchaseRequisitionItem (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    HeaderId INT NOT NULL,
    ItemNo VARCHAR(50) NOT NULL,
    ShortText TEXT NOT NULL,
    UnitOfMeasure VARCHAR(50) NOT NULL,
    Quantity DECIMAL(18,2) NOT NULL,
    ValuationPrice DECIMAL(18,2) NOT NULL,
    TotalValue DECIMAL(18,2) NOT NULL,
    DeliveryDate DATETIME NOT NULL,
    MaterialGroup VARCHAR(255) NOT NULL,
    PlantCode VARCHAR(100) NOT NULL,
    PurchasingGroup VARCHAR(100) NOT NULL,
    Requisitioner VARCHAR(255) NOT NULL,
    QuantityAccountAssignment DECIMAL(18,2) NOT NULL,
    CostCentre VARCHAR(100) NOT NULL,
    GLAccount VARCHAR(100) NOT NULL,
    CostCentreBearer VARCHAR(100) NOT NULL,
    CONSTRAINT FK_PRItem_Header FOREIGN KEY (HeaderId)
        REFERENCES PurchaseRequisitionHeader (Id)
        ON DELETE CASCADE
);
