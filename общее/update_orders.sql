-- Расширение существующей БД. Применить один раз к chudoobuv.
-- Размеры, количества и цены восстанавливаются из оригинального Orders_import.xlsx.
USE chudoobuv;
ALTER TABLE ordersitem
    ADD COLUMN sizeId INT NULL,
    ADD COLUMN quantity INT NULL,
    ADD COLUMN unitPrice DECIMAL(10,2) NULL,
    ADD CONSTRAINT fk_OrderItemSize FOREIGN KEY (sizeId) REFERENCES sizes(idSize) ON DELETE RESTRICT,
    ADD CONSTRAINT ck_OrderItemQuantity CHECK (quantity IS NULL OR quantity > 0),
    ADD CONSTRAINT ck_OrderItemPrice CHECK (unitPrice IS NULL OR unitPrice >= 0);

-- Восстановление полных данных из оригинального Orders_import.xlsx.
-- Остатки уже импортированы как снимок наличия; повторно их не списываем.
USE chudoobuv;
START TRANSACTION;
UPDATE ordersitem SET sizeId=3, quantity=1, unitPrice=2190 WHERE idOrderItem=1 AND sizeId IS NULL AND quantity IS NULL AND unitPrice IS NULL;
UPDATE ordersitem SET sizeId=5, quantity=1, unitPrice=2790 WHERE idOrderItem=2 AND sizeId IS NULL AND quantity IS NULL AND unitPrice IS NULL;
UPDATE ordersitem SET sizeId=19, quantity=1, unitPrice=25000 WHERE idOrderItem=3 AND sizeId IS NULL AND quantity IS NULL AND unitPrice IS NULL;
UPDATE ordersitem SET sizeId=23, quantity=1, unitPrice=7500 WHERE idOrderItem=4 AND sizeId IS NULL AND quantity IS NULL AND unitPrice IS NULL;
UPDATE ordersitem SET sizeId=21, quantity=1, unitPrice=17000 WHERE idOrderItem=5 AND sizeId IS NULL AND quantity IS NULL AND unitPrice IS NULL;
UPDATE ordersitem SET sizeId=21, quantity=1, unitPrice=8565 WHERE idOrderItem=6 AND sizeId IS NULL AND quantity IS NULL AND unitPrice IS NULL;
UPDATE ordersitem SET sizeId=18, quantity=1, unitPrice=7795 WHERE idOrderItem=7 AND sizeId IS NULL AND quantity IS NULL AND unitPrice IS NULL;
UPDATE ordersitem SET sizeId=23, quantity=1, unitPrice=7757 WHERE idOrderItem=8 AND sizeId IS NULL AND quantity IS NULL AND unitPrice IS NULL;
UPDATE ordersitem SET sizeId=23, quantity=1, unitPrice=12340 WHERE idOrderItem=9 AND sizeId IS NULL AND quantity IS NULL AND unitPrice IS NULL;
UPDATE ordersitem SET sizeId=25, quantity=1, unitPrice=7130 WHERE idOrderItem=10 AND sizeId IS NULL AND quantity IS NULL AND unitPrice IS NULL;
UPDATE ordersitem SET sizeId=19, quantity=1, unitPrice=8765 WHERE idOrderItem=11 AND sizeId IS NULL AND quantity IS NULL AND unitPrice IS NULL;
UPDATE ordersitem SET sizeId=25, quantity=1, unitPrice=15000 WHERE idOrderItem=12 AND sizeId IS NULL AND quantity IS NULL AND unitPrice IS NULL;
UPDATE ordersitem SET sizeId=29, quantity=1, unitPrice=9567 WHERE idOrderItem=13 AND sizeId IS NULL AND quantity IS NULL AND unitPrice IS NULL;
UPDATE ordersitem SET sizeId=29, quantity=1, unitPrice=21500 WHERE idOrderItem=14 AND sizeId IS NULL AND quantity IS NULL AND unitPrice IS NULL;
UPDATE ordersitem SET sizeId=25, quantity=1, unitPrice=7890 WHERE idOrderItem=15 AND sizeId IS NULL AND quantity IS NULL AND unitPrice IS NULL;
UPDATE ordersitem SET sizeId=27, quantity=1, unitPrice=24569 WHERE idOrderItem=16 AND sizeId IS NULL AND quantity IS NULL AND unitPrice IS NULL;
UPDATE ordersitem SET sizeId=31, quantity=1, unitPrice=14567 WHERE idOrderItem=17 AND sizeId IS NULL AND quantity IS NULL AND unitPrice IS NULL;
UPDATE ordersitem SET sizeId=29, quantity=1, unitPrice=15670 WHERE idOrderItem=18 AND sizeId IS NULL AND quantity IS NULL AND unitPrice IS NULL;
UPDATE ordersitem SET sizeId=1, quantity=1, unitPrice=4290 WHERE idOrderItem=19 AND sizeId IS NULL AND quantity IS NULL AND unitPrice IS NULL;
UPDATE ordersitem SET sizeId=4, quantity=2, unitPrice=2190 WHERE idOrderItem=20 AND sizeId IS NULL AND quantity IS NULL AND unitPrice IS NULL;
UPDATE ordersitem SET sizeId=11, quantity=1, unitPrice=1690 WHERE idOrderItem=21 AND sizeId IS NULL AND quantity IS NULL AND unitPrice IS NULL;
UPDATE ordersitem SET sizeId=34, quantity=1, unitPrice=25625 WHERE idOrderItem=22 AND sizeId IS NULL AND quantity IS NULL AND unitPrice IS NULL;
UPDATE ordersitem SET sizeId=27, quantity=3, unitPrice=7890 WHERE idOrderItem=23 AND sizeId IS NULL AND quantity IS NULL AND unitPrice IS NULL;
UPDATE ordersitem SET sizeId=33, quantity=2, unitPrice=6550 WHERE idOrderItem=24 AND sizeId IS NULL AND quantity IS NULL AND unitPrice IS NULL;
UPDATE ordersitem SET sizeId=16, quantity=1, unitPrice=2490 WHERE idOrderItem=25 AND sizeId IS NULL AND quantity IS NULL AND unitPrice IS NULL;
UPDATE ordersitem SET sizeId=5, quantity=2, unitPrice=3190 WHERE idOrderItem=26 AND sizeId IS NULL AND quantity IS NULL AND unitPrice IS NULL;
UPDATE ordersitem SET sizeId=18, quantity=1, unitPrice=7550 WHERE idOrderItem=27 AND sizeId IS NULL AND quantity IS NULL AND unitPrice IS NULL;
UPDATE ordersitem SET sizeId=21, quantity=1, unitPrice=7130 WHERE idOrderItem=28 AND sizeId IS NULL AND quantity IS NULL AND unitPrice IS NULL;
UPDATE ordersitem SET sizeId=19, quantity=1, unitPrice=12340 WHERE idOrderItem=29 AND sizeId IS NULL AND quantity IS NULL AND unitPrice IS NULL;
UPDATE ordersitem SET sizeId=18, quantity=2, unitPrice=8765 WHERE idOrderItem=30 AND sizeId IS NULL AND quantity IS NULL AND unitPrice IS NULL;
COMMIT;

ALTER TABLE ordersitem
    MODIFY sizeId INT NOT NULL,
    MODIFY quantity INT NOT NULL,
    MODIFY unitPrice DECIMAL(10,2) NOT NULL;
ALTER TABLE stockitems
    ADD CONSTRAINT uq_StockProductSize UNIQUE (prodId, SizeId),
    ADD CONSTRAINT ck_StockQuantity CHECK (quantity IS NULL OR quantity >= 0);

-- Модель, размер и остаток обязательны.
-- Перед выполнением: все три поля должны быть заполнены.
ALTER TABLE stockitems
    MODIFY prodId INT NOT NULL,
    MODIFY SizeId INT NOT NULL,
    MODIFY quantity INT NOT NULL;
