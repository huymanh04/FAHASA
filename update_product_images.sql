-- Script để update hình ảnh cho tất cả sản phẩm
-- Gán random các file ảnh khác nhau cho mỗi sản phẩm

DECLARE @images TABLE (Id INT IDENTITY(1,1), ImageName NVARCHAR(200))
INSERT INTO @images (ImageName) VALUES
('000f93f4-27b2-43b3-aa8b-bea069819c38.jpg'),
('014a1913-926d-40ef-b6b4-a6bfea4920a9.jpg'),
('022fc80e-0777-4aea-9e9b-da2cd8f66630.jpg'),
('03624c03-ea74-480f-acaf-4fc8c4a43f22.jpg'),
('036572d5-26b0-4c93-a94e-d47c717a31a4.jpg'),
('038232e4-202a-4ab5-add9-0935282ad1ad.jpg'),
('0879e183-9e71-4684-85fb-0010532baf6b.jpg'),
('0bfe78e3-2b06-4c13-80e7-512c9d7779a0.jpg'),
('0f008510-a645-436d-8c32-23c3562b5f26.jpg'),
('0f9a5f9a-2c2e-4dfc-a94e-b1d5789c4129.jpg'),
('10687ca4-0cb6-4912-acb1-9ce26df4aac8.jpg'),
('146e15ec-18c9-4561-b36a-b8ba2124d4ed.jpg'),
('15ea4fa9-2063-4885-b573-271953b26966.jpg'),
('17ae76aa-47af-4fca-8f8a-fe231b70fd64.jpg'),
('18679eeb-9976-4140-975a-513c10424265.jpg'),
('18c3eacb-d6f6-497a-88bc-fff1ed114595.jpg'),
('1a9d228e-6ade-4408-8455-463cb75d01c1.jpg'),
('1bd07059-0847-4347-85bf-f098943e3692.jpg'),
('21bc4f25-0224-470e-b82c-5077e65f322b.jpg'),
('21c0508f-43c4-44c3-b16d-439563b2b839.jpg'),
('2354e1b8-f571-4bbf-9d02-e3ff841f8541.jpg'),
('235c3b7c-df1f-4ba1-be68-2a218e3b00c3.jpg'),
('24ec3a3a-7ca9-4520-9c1b-091b4a879dca.jpg'),
('25d96a74-bf75-44a6-8e6e-eaa215d52226.jpg'),
('27712903-6c7a-4822-820e-caff209681d8.jpg'),
('2a78d17c-35ce-4d5e-ba5d-a33e0c508c2f.jpg'),
('2bf50c17-a2f9-4545-91d0-f698eab8cbb7.jpg'),
('2d2c72dd-0b2b-4021-bbdc-b324e28a3c7d.jpg'),
('2fb4c469-639a-4e4f-b421-e27b7d43233f.jpg'),
('316dd8eb-7b72-4043-b5d6-bc1f4781caa2.jpg'),
('31cfd8d5-e48b-42de-be38-bda8139269b5.jpg'),
('34a158c1-5535-43f2-a32c-04ee92c7cc63.jpg'),
('3685ff13-0761-4db1-b106-3965a2b7a73a.jpg'),
('369e78b7-dbd7-4fcc-8f90-0b563cdb0356.jpg'),
('378e92ce-c2b8-4fad-bc34-78f8ea4f2ac8.jpg'),
('37fe04c7-97d2-4921-b2cd-42acd2b7e594.jpg'),
('3825e3c4-55c3-4d46-8d42-3a33c63e588a.jpg'),
('390d59f5-e723-4ec7-8b4d-ea5710b7b902.jpg'),
('39eb97b9-913f-4822-bf64-ab550584e3b7.jpg'),
('3af7f133-def7-4d34-bc6d-697a3cd2e8b3.jpg'),
('3cdce504-a9eb-4db8-9e4a-2d80ab425bc2.jpg'),
('3df05301-b7e3-41a2-9030-006c646b7c5b.jpg'),
('3e5391bd-d1d3-4fa5-9266-8cf4c7056784.jpg'),
('3eb5fdfa-7155-4a72-a903-35691f6d88ee.jpg'),
('406e8967-0122-417d-92d5-8da89c9edb38.jpg'),
('4242058e-6065-47b3-a67f-cc7c771d191e.jpg'),
('431fb7a2-e5af-49f4-ac33-14470b11d30f.jpg'),
('437127ae-91bd-4f01-9496-76008e0bd2d6.jpg'),
('44834ede-56f8-4ea1-9d7d-10197a232df3.jpg'),
('44ab0b31-738b-4387-a408-bfa5bc6b606f.jpg'),
('4628cc59-3618-4adc-a06b-32680756bf50.jpg'),
('4868289f-0030-45d2-9820-998545e62130.jpg'),
('488323d0-8c88-4491-b096-f19d7066a865.jpg'),
('489d7f5b-1b39-4514-9eb9-6b50df635e66.jpg'),
('48cdfedf-0ea8-4c41-b01d-20f31fda6faf.jpg'),
('491c3933-b56a-4760-bab1-8e2e80383dfd.jpg'),
('4bffd9f3-4ed2-4cfc-999f-218fcdad796f.jpg'),
('51ddaffb-aca3-4dc2-98a8-cdb38161fcda.jpg'),
('5516cc81-e806-4d6b-ab72-fc59e4ab5d56.jpg'),
('5573efa0-4357-4303-ad22-66baed172b3b.jpg');

-- Update từng sản phẩm với hình ảnh khác nhau
WITH ProductsWithRowNum AS (
    SELECT ProductID, ROW_NUMBER() OVER (ORDER BY ProductID) AS RowNum
    FROM Products
)
UPDATE p
SET Image = i.ImageName
FROM Products p
INNER JOIN ProductsWithRowNum pr ON p.ProductID = pr.ProductID
INNER JOIN @images i ON (pr.RowNum - 1) % 60 + 1 = i.Id;

SELECT COUNT(*) AS 'Số sản phẩm đã update' FROM Products WHERE Image IS NOT NULL;
