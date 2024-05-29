-- 4. Создать представление, через которое невозможно получить информацию о том,
-- какая конкретно книга была выдана читателю в любой из выдач.
CREATE	OR	REPLACE	VIEW	subscriptions_bookless	AS
	SELECT
		sb_id,
		sb_subscriber,
		sb_start,
		sb_finish,
		sb_is_active
	FROM
		subscriptions;

SELECT * FROM subscriptions_bookless;


-- 17. Создать триггер, меняющий дату выдачи книги на текущую, если указанная в
-- INSERT- или UPDATE-запросе дата выдачи книги меньше текущей на полгода и
-- более.

-- DROP	TRIGGER	`data_control_ins`;
DELIMITER	$$
CREATE	TRIGGER	`data_control_ins`
	BEFORE	INSERT	ON	subscriptions
	FOR	EACH	ROW
		BEGIN
			IF (DATEDIFF(NOW(), NEW.sb_start) > 182)
			THEN
				SET NEW.`sb_start` = NOW();
			END IF;
		END$$
DELIMITER	;

-- DROP	TRIGGER	`data_control_upd`;
DELIMITER	$$
CREATE	TRIGGER	`data_control_upd`
	BEFORE	UPDATE	ON	subscriptions
	FOR	EACH	ROW
		BEGIN
			IF (DATEDIFF(NOW(), NEW.sb_start) > 182)
			THEN
				SET NEW.`sb_start` = NOW();
			END IF;
		END$$
DELIMITER	;


-- 16. Создать триггер, корректирующий название книги таким образом, чтобы оно
-- удовлетворяло следующим условиям:
-- a. не допускается наличие пробелов в начале и конце названия;
-- b. не допускается наличие повторяющихся пробелов;
-- c. первая буква в названии всегда должна быть заглавной.

-- DROP	TRIGGER	`name_control_ins`;

DELIMITER	$$
CREATE	TRIGGER	`name_control_ins`
	BEFORE	INSERT	ON	books
	FOR	EACH	ROW
		BEGIN
		    SET NEW.b_name = TRIM(NEW.b_name);
		    SET NEW.b_name = REGEXP_REPLACE(NEW.b_name, ' {2,}', ' ');
		    SET NEW.b_name = CONCAT(UPPER(LEFT(NEW.b_name, 1)), SUBSTRING(NEW.b_name, 2));
		END$$
DELIMITER	;

-- DROP	TRIGGER	`name_control_upd`;

DELIMITER	$$
CREATE	TRIGGER	`name_control_upd`
	BEFORE	UPDATE	ON	books
	FOR	EACH	ROW
		BEGIN
		    SET NEW.b_name = TRIM(NEW.b_name);
		    SET NEW.b_name = REGEXP_REPLACE(NEW.b_name, ' {2,}', ' ');
		    SET NEW.b_name = CONCAT(UPPER(LEFT(NEW.b_name, 1)), SUBSTRING(NEW.b_name, 2));
		END$$
DELIMITER	;



-- 14. Создать триггер, не позволяющий выдать книгу читателю, у которого на руках
-- находится пять и более книг, при условии, что суммарное время, оставшееся до
-- возврата всех выданных ему книг, составляет менее одного месяца.

-- DROP	TRIGGER	`subs_control_ins`;

DELIMITER	$$
CREATE	TRIGGER	`subs_control_ins`
	BEFORE	INSERT	ON	subscriptions
	FOR	EACH	ROW
		BEGIN
		        DECLARE book_count INT;
			    DECLARE remaining_time INT;

			    SELECT COUNT(*) INTO book_count
				    FROM subscriptions
				    WHERE sb_subscriber = NEW.sb_subscriber AND sb_is_active="Y";

			    SELECT SUM(DATEDIFF(sb_start, CURDATE())) INTO remaining_time
				    FROM subscriptions
				    WHERE sb_subscriber = NEW.sb_subscriber AND sb_is_active="Y";

			    IF book_count >= 5 AND remaining_time < 30 THEN
			        SIGNAL SQLSTATE '45000'
			       	SET MESSAGE_TEXT = 'Reader has reached the limit',
			       			MYSQL_ERRNO = 1002;
			    END IF;
		END$$
DELIMITER	;



-- 15. Создать триггер, допускающий регистрацию в библиотеке только таких авторов,
-- имя которых не содержит никаких символов кроме букв, цифр, знаков - (минус), '
-- (апостроф) и пробелов (не допускается два и более идущих подряд пробела).


-- DROP	TRIGGER	`author_name_control_ins`;

DELIMITER	$$
CREATE	TRIGGER	`author_name_control_ins`
	BEFORE	INSERT	ON	authors
	FOR	EACH	ROW
		BEGIN
		        DECLARE invalid_characters VARCHAR(255);
    		    SET invalid_characters = '[^a-zA-Zа-яА-Я0-9\s\'-]';
			    IF NEW.a_name REGEXP invalid_characters OR NEW.a_name REGEXP '  ' THEN
			        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Invalid author name', MYSQL_ERRNO = 1001;
			    END IF;
		END$$
DELIMITER	;


