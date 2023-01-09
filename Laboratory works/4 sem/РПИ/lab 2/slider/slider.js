    function writeCookie(name, val, expires) {
        var date = new Date;
        date.setDate(date.getDate() + expires);
        document.cookie = name+"="+val+"; path=/; expires=" + date.toUTCString();
    }

    function readCookie(name) {
        var matches = document.cookie.match(new RegExp(
            "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
        ));
        return matches ? decodeURIComponent(matches[1]) : undefined;
    }

    let position;
    if (document.cookie.split(';').filter((item) => item.trim().startsWith('position=')).length) {
        position = readCookie('position');
    }
    else position = 0;

    const slidesToShow = 1;
    const slidesToScroll = 1;
    const container = document.querySelector(`.slider-container`);
    const track = document.querySelector('.slider-track');
    const item = document.querySelector('.slider-item');

    const btnPrev = document.querySelector('.btn-prev');
    const btnNext = document.querySelector('.btn-next');

    const itemWidth = container.clientWidth / slidesToShow;
    const movePosition = slidesToScroll * itemWidth;
    const items = document.querySelectorAll('.slider-item');
    const itemCount = items.length;

    const dotHolder = document.querySelector('.slider-dot-holder');

    var curItem = ( - position ) / itemWidth;


    items.forEach((item) => {
       item.style.minWidth = `${itemWidth}px`;
    });


    const setPosition = () => {

        if ( position <= - ( container.clientWidth * itemCount ) )
            position = 0;
        if ( position > 0 )
            position = - ( container.clientWidth * ( itemCount - 1 ) );

        track.style.transform = `translateX(${position}px)`;
        curItem = ( - position ) / itemWidth;
        writeCookie('position', `${position}`, 30);
    };

    setPosition();


    const setDots = () => {

        while ( dotHolder.hasChildNodes() )
        {
            dotHolder.removeChild( dotHolder.childNodes[0] );
        }

        for ( let i = 0; i < itemCount; i++ )
        {
            const dot = document.createElement("div");
            dot.classList.add('slider-dot');
            if ( i === curItem )
                dot.style.backgroundColor = "#3949ab";
            else
                dot.style.backgroundColor = "#00897b";
            dotHolder.appendChild(dot);
        }
    }

    let timerId = setInterval( () => {
        position -= movePosition;
        setPosition();
        setDots();
    }, 7000);

    setDots();



