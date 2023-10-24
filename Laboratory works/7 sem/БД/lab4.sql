-- 10. Добавить в базу данных жанры «Политика», «Психология», «История».
INSERT	IGNORE	INTO	`genres`	(`g_name`)
VALUES	('Политика'),
        ('Психология'),
        ('История');


-- 8. Удалить все книги, относящиеся к жанру «Классика».
SET FOREIGN_KEY_CHECKS = 0;
DELETE  `books`, `m2m_books_genres`
FROM    `books`
    JOIN `m2m_books_genres` ON `books`.`b_id` = `m2m_books_genres`.`b_id`
    JOIN `genres` ON `m2m_books_genres`.`g_id` = `genres`.`g_id`
WHERE   `genres`.`g_name` = 'Классика';
SET FOREIGN_KEY_CHECKS = 1;


-- 7. Удалить информацию обо всех выдачах читателям книги с идентификатором 1.
DELETE FROM subscriptions
WHERE sb_book = 1;

-- 6. Отметить как невозвращённые все выдачи, полученные читателем с идентификатором 2.
UPDATE	subscriptions
SET sb_is_active = 'Y'
WHERE sb_subscriber = 2;


-- 5. Для всех выдач, произведённых до 1-го января 2012-го года, уменьшить значение дня выдачи на 3.
UPDATE	subscriptions
SET	sb_start = DATE_SUB(sb_start, INTERVAL 3 DAY)
WHERE sb_start < "2012-01-01";
