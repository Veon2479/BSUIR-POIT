-- 1. Создать хранимую функцию, получающую на вход идентификатор читателя и
-- возвращающую список идентификаторов книг, которые он уже прочитал и вернул в
-- библиотеку.
DROP FUNCTION IF EXISTS GetReadBooks;

DELIMITER //
CREATE FUNCTION GetReadBooks(rd_id INT) RETURNS VARCHAR(255)
BEGIN
    DECLARE book_list VARCHAR(255);
    SELECT GROUP_CONCAT(sb_book) INTO book_list
    FROM subscriptions
    WHERE sb_subscriber = rd_id AND sb_is_active = 'N';
    RETURN book_list;
END //
DELIMITER ;

SELECT GetReadBooks(1) AS book_list;


-- 3. Создать хранимую функцию, получающую на вход идентификатор читателя и
-- возвращающую 1, если у читателя на руках сейчас менее десяти книг, и 0 в
-- противном случае.
DROP FUNCTION IF EXISTS CheckBorrowedBooksCount;

DELIMITER //
CREATE FUNCTION CheckBorrowedBooksCount(rd_id INT) RETURNS INT
BEGIN
    DECLARE books_count INT;

    SELECT COUNT(*) INTO books_count
    FROM subscriptions
    WHERE sb_subscriber = rd_id AND sb_is_active = 'Y';

    IF books_count < 10 THEN
        RETURN 1;
    ELSE
        RETURN 0;
    END IF;
END //
DELIMITER ;

SELECT CheckBorrowedBooksCount(3) AS bor_count;

-- 4. Создать хранимую функцию, получающую на вход год издания книги и
-- возвращающую 1, если книга издана менее ста лет назад, и 0 в противном случае.
DROP FUNCTION IF EXISTS CheckBookPublicationYear;

DELIMITER //
CREATE FUNCTION CheckBookPublicationYear(year_of_publication INT) RETURNS INT
BEGIN
    DECLARE current_year INT;
    DECLARE is_within_hundred_years INT;

    SET current_year = YEAR(NOW());

    IF year_of_publication >= current_year - 100 THEN
        SET is_within_hundred_years = 1;
    ELSE
        SET is_within_hundred_years = 0;
    END IF;

    RETURN is_within_hundred_years;
END //
DELIMITER ;

SELECT CheckBookPublicationYear(2023) as res;



-- 10. Создать хранимую процедуру, удаляющую все индексы (кроме первичных ключей),
-- построенные на таблицах текущей базы данных и включающие в себя более
-- одного поля.
DELIMITER //

CREATE OR REPLACE PROCEDURE DropNonPrimaryIndexes()
BEGIN
    DECLARE done INT DEFAULT FALSE;
    DECLARE table_name VARCHAR(255);
    DECLARE index_name VARCHAR(255);

    DECLARE tables_cursor CURSOR FOR
        SELECT table_name
        FROM information_schema.tables
        WHERE table_schema = DATABASE();

    DECLARE indexes_cursor CURSOR FOR
        SELECT table_name, index_name
        FROM information_schema.statistics
        WHERE table_schema = DATABASE()
            AND non_unique = 1;

    DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;

    OPEN tables_cursor;
    tables_loop: LOOP
        FETCH tables_cursor INTO table_name;
        IF done THEN
            LEAVE tables_loop;
        END IF;

        OPEN indexes_cursor;
        indexes_loop: LOOP
            FETCH indexes_cursor INTO table_name, index_name;
            IF done THEN
                LEAVE indexes_loop;
            END IF;

            IF table_name = table_name AND (SELECT COUNT(DISTINCT column_name) FROM information_schema.statistics WHERE table_schema = DATABASE() AND table_name = table_name AND index_name = index_name) > 1 THEN
                SET @drop_index_sql = CONCAT('DROP INDEX ', index_name, ' ON ', table_name);
                PREPARE drop_stmt FROM @drop_index_sql;
                EXECUTE drop_stmt;
                DEALLOCATE PREPARE drop_stmt;
            END IF;
        END LOOP;

        CLOSE indexes_cursor;
    END LOOP;

    CLOSE tables_cursor;
END //

DELIMITER ;

CALL DropNonPrimaryIndexes;



-- 6. Создать хранимую процедуру, формирующую список таблиц и их внешних ключей,
-- зависящих от указанной в параметре функции таблицы.
DELIMITER //

CREATE OR REPLACE PROCEDURE ShowTablesWithForeignKeys(IN target_table_name VARCHAR(255))
BEGIN
    DECLARE done INT DEFAULT FALSE;
    DECLARE table_name VARCHAR(255);
    DECLARE foreign_key_name VARCHAR(255);

    -- Cursor to fetch tables and their foreign keys
    DECLARE tables_cursor CURSOR FOR
        SELECT DISTINCT
            fk.table_name,
            fk.constraint_name
        FROM information_schema.referential_constraints fk
        WHERE fk.referenced_table_name = target_table_name;

    -- Error handling
    DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;

    -- Display header
    SELECT 'Table Name', 'Foreign Key Name';
    SELECT '------------------------', '------------------------';

    -- Loop through tables and display
    OPEN tables_cursor;
    tables_loop: LOOP
        FETCH tables_cursor INTO table_name, foreign_key_name;
        IF done THEN
            LEAVE tables_loop;
        END IF;

        SELECT table_name, foreign_key_name;
    END LOOP;

    CLOSE tables_cursor;
END //

DELIMITER ;

CALL ShowTablesWithForeignKeys('authors');
