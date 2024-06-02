function ShowDock() {
    document.getElementById("icon-container").onmousemove = e => {
        e.currentTarget.querySelectorAll("li").forEach((li) => {
            // 计算每个图标中点横坐标: centroid
            const centroid = li.offsetLeft + li.offsetWidth / 2;
            // 根据鼠标与图标中点的横坐标偏移量计算当前的--scale
            li.style.setProperty("--scale", curve(e.clientX - centroid));
        });
    };

    document.getElementById("icon-container").onmouseleave = e => {
        e.currentTarget.querySelectorAll("li").forEach((li) => {
            // 鼠标移开，放大倍数归1
            li.style.setProperty("--scale", 1);
        });
    };
}

function isWeiXin() {
    const ua = navigator.userAgent
    return !!/MicroMessenger/i.test(ua)
}

function isPhone() {
    const u = navigator.userAgent;
    const isAndroid = u.indexOf('Android') > -1 || u.indexOf('Adr') > -1; //android终端
    const isiOS = !!u.match(/\(i[^;]+;( U;)? CPU.+Mac OS X/); //ios终端
    return isAndroid || isiOS;
}

function NavigateTo(url, webUrl) {
    let result = isPhone()
    if (result) {
        window.open(url)
    } else {
        window.open(webUrl)
    }
}

function jsSaveAsFile(filename, byteBase64) {
    const link = document.createElement('a');
    link.download = filename;
    link.href = "data:application/octet-stream;base64," + byteBase64;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

function getDimensions() {
    return 768 >= window.innerWidth;
}