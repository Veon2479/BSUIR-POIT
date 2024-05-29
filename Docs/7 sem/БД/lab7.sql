-- 2. Создать хранимую процедуру, которая:
-- a. увеличивает значение поля «b_quantity» для всех книг в два раза;
-- b. отменяет совершённое действие, если по итогу выполнения операции
-- среднее количество экземпляров книг превысит значение 50.
DELIMITER //
CREATE OR REPLACE  PROCEDURE UpdateBookQuantity()
BEGIN
    DECLARE avgQuantity DECIMAL(10, 2);
    START TRANSACTION;
    UPDATE books
    SET b_quantity = b_quantity * 2;
    SELECT AVG(b_quantity) INTO avgQuantity
    FROM books;
    IF avgQuantity > 50 THEN
        ROLLBACK;
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Average quantity exceeds 50. Transaction rolled back.';
    ELSE
        COMMIT;
    END IF;
END //
DELIMITER ;


-- 6. Создать на таблице «subscriptions» триггер, определяющий уровень
-- изолированности транзакции, в котором сейчас проходит операция обновления, и
-- отменяющий операцию, если уровень изолированности транзакции отличен от
-- REPEATABLE READ.
DROP TRIGGER IF EXISTS CheckIsolationLevel;
DELIMITER //
CREATE TRIGGER CheckIsolationLevel
BEFORE UPDATE ON subscriptions
FOR EACH ROW
BEGIN
    DECLARE currentIsolation VARCHAR(50);
    SELECT @@tx_isolation INTO currentIsolation;
    IF currentIsolation != "REPEATABLE-READ" THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Transaction isolation level is not REPEATABLE READ. Operation canceled.';
    END IF;
END //
DELIMITER ;


-- 8. Создать хранимую процедуру, выполняющую подсчёт количества записей в
-- указанной таблице таким образом, чтобы она возвращала максимально
-- корректные данные, даже если для достижения этого результата придётся
-- пожертвовать производительностью.
DELIMITER //
CREATE OR REPLACE PROCEDURE CountRows(IN tableName VARCHAR(50), OUT rowCount INT)
BEGIN
    SET TRANSACTION ISOLATION LEVEL REPEATABLE READ;
    SET @table = tableName;
    SET @sql = CONCAT('SELECT COUNT(*) INTO @rc FROM ', tableName);
    PREPARE stmt FROM @sql;
    EXECUTE stmt;
    DEALLOCATE PREPARE stmt;
   SET rowCount = @rc;
   COMMIT;
END //
DELIMITER ;


-- 5. Написать код, в котором запрос, инвертирующий значения поля «sb_is_active»
-- таблицы «subscriptions» с «Y» на «N» и наоборот, будет иметь максимальные
-- шансы на успешное завершение в случае возникновения ситуации взаимной
-- блокировки с другими транзакциями.
DELIMITER //
CREATE OR REPLACE PROCEDURE InverseStatus()
BEGIN
   	SET TRANSACTION ISOLATION LEVEL REPEATABLE READ;
   		UPDATE subscriptions
		SET sb_is_active = CASE WHEN sb_is_active = 'Y' THEN 'N' ELSE 'Y' END;
   	COMMIT;
END //
DELIMITER ;

-- 1. Создать хранимую процедуру, которая:
-- a. добавляет каждой книге два случайных жанра;
-- b. отменяет совершённые действия, если в процессе работы хотя бы одна
-- операция вставки завершилась ошибкой в силу дублирования значения
-- первичного ключа таблицы «m2m_books_genres» (т.е. у такой книги уже был
-- такой жанр).
DELIMITER //
CREATE OR REPLACE PROCEDURE RndGenres()
BEGIN
    DECLARE genre1 INT;
    DECLARE genre2 INT;
    DECLARE book_id INT;

    DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
    END;

    START TRANSACTION;
	    FOR book_id IN (SELECT b_id FROM books) DO
	        SET genre1 = (SELECT g_id FROM genres ORDER BY RAND() LIMIT 1);
	        SET genre2 = (SELECT g_id FROM genres WHERE g_id != genre1 ORDER BY RAND() LIMIT 1);
	        INSERT INTO m2m_books_genres (b_id, g_id) VALUES (book_id, genre1), (book_id, genre2);
	    END FOR;
    COMMIT;
END //
DELIMITER ;
