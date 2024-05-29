-- 15. Показать, сколько в среднем экземпляров книг есть в библиотеке.
SELECT COUNT(`b_quantity`) as `books_avg`
FROM `books`;

-- 6. Показать, сколько всего раз читателям выдавались книги.
SELECT COUNT(`sb_id`)
FROM `subscriptions`;

-- 7. Показать, сколько читателей брало книги в библиотеке.
SELECT COUNT( DISTINCT `sb_subscriber`) as `users`
FROM `subscriptions`;

-- 13. Показать идентификаторы всех «самых читающих читателей», взявших в
-- библиотеке больше всего книг.
SELECT `sb_subscriber`
FROM	(	SELECT 	`sb_subscriber`,
							COUNT(*) as `book_count`
				FROM `subscriptions`
				GROUP BY `sb_subscriber`) as `table`
WHERE `book_count` = (  SELECT MAX(`book_count`)
                        FROM (	SELECT 	`sb_subscriber`,
                                        COUNT(*) as `book_count`
                                FROM `subscriptions`
                                GROUP BY `sb_subscriber`) as `table`)

-- 11. Показать идентификаторы и даты выдачи книг за первый год работы библиотеки
-- (первым годом работы библиотеки считать все даты с первой выдачи книги по 31-е
-- декабря (включительно) того года, когда библиотека начала работать).


SELECT	`sb_book`,
				`sb_start`
FROM	`subscriptions`
WHERE	`sb_start`
		BETWEEN
				(SELECT MIN(`sb_start`)
				FROM `subscriptions`)
		AND
				(SELECT STR_TO_DATE(CONCAT(
						(SELECT CONVERT(YEAR(MIN(`sb_start`)), CHAR)
						FROM `subscriptions`), '-12-31'), '%Y-%m-%d'
				))

