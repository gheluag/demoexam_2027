-- Перед выполнением: все три поля должны быть заполнены.
USE chudoobuv;
ALTER TABLE stockitems
    MODIFY prodId INT NOT NULL,
    MODIFY SizeId INT NOT NULL,
    MODIFY quantity INT NOT NULL;
