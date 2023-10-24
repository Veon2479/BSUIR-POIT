-- 23. Показать читателя, последним взявшего в библиотеке книгу.

SELECT `s_name`
FROM
	(SELECT	`subscribers`.`s_name`,
					`sb_start`
	FROM	`subscriptions`
				JOIN	`subscribers`
						ON	`subscriptions`.`sb_subscriber`  = `subscribers`.`s_id`
	ORDER BY	`sb_start` DESC
	LIMIT 1) as `tbl`



-- 25. Показать, какую книгу (или книги, если их несколько) каждый читатель взял в свой
-- последний визит в библиотеку.

SELECT	`s_name`,
				`subscriptions`.`sb_subscriber`,
				`sb_start`,
				`b_name`
FROM (
    SELECT	`sb_subscriber`,
            MAX(`sb_start`) AS `latest_start`
    FROM	`subscriptions`
    GROUP BY `sb_subscriber`
) AS `latest`
    JOIN `subscriptions`
            ON `subscriptions`.`sb_subscriber` = `latest`.`sb_subscriber`
            AND `subscriptions`.`sb_start` = `latest`.`latest_start`
    JOIN `subscribers`
            ON `subscriptions`.`sb_subscriber` = `subscribers`.`s_id`
    JOIN `books`
            ON `subscriptions`.`sb_book` = `books`.`b_id`
    ORDER BY `subscriptions`.`sb_start` DESC




-- 24. Показать читателя (или читателей, если их окажется несколько), дольше всего
-- держащего у себя книгу (учитывать только случаи, когда книга не возвращена).

SELECT	`s_name`
FROM	`subscribers`
		JOIN `subscriptions`
				ON `s_id` = `sb_subscriber`
WHERE
	`sb_is_active` = 'Y'
	AND DATEDIFF(NOW(), `sb_start`) =
(
		SELECT	DATEDIFF(NOW(), `sb_start`) as `diff`
		FROM
		(
				SELECT	`sb_start`,
								`sb_finish`
				FROM	`subscriptions`
				WHERE `sb_is_active` = 'Y'
		) as `dd`
		ORDER BY `diff` DESC
		LIMIT 1
)



-- 21. Показать читателей, бравших самые разножанровые книги (т.е. книги,
-- одновременно относящиеся к максимальному количеству жанров).

SELECT
	`s_name`
FROM
	(
		SELECT
			`sb_subscriber`,
			`s_name`
		FROM
			`subscriptions`
				JOIN	`books`	ON	`sb_book` = `b_id`
				JOIN	`m2m_books_genres`	USING(`b_id`)
				JOIN	`subscribers`	ON `sb_subscriber` = `s_id`
		GROUP BY `sb_id`
		HAVING COUNT(`g_id`) =
				(
					SELECT
						COUNT(`g_id`) as `cnt`
					FROM
						`books`
							JOIN	`m2m_books_genres`	USING(`b_id`)
					GROUP BY
						`b_id`
					ORDER BY
						`cnt`
						DESC
					LIMIT
						1
				)
	)	as	`data`
GROUP BY
	`sb_subscriber`




-- 15. Показать всех авторов и количество книг (не экземпляров книг, а «книг как
-- изданий») по каждому автору.

SELECT
	`a_name`,
	COUNT(`b_id`) as `cnt`
FROM
	`authors`
		JOIN `m2m_books_authors` USING(`a_id`)
		JOIN `books` USING(`b_id`)
GROUP BY
	`a_id`
