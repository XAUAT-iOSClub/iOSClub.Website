function isWeiXin() {
    const ua = navigator.userAgent
    return !!/MicroMessenger/i.test(ua)
}

function NavigateTo(url, webUrl) {
    const u = navigator.userAgent;
    const isAndroid = u.indexOf('Android') > -1 || u.indexOf('Adr') > -1; //android终端
    const isiOS = !!u.match(/\(i[^;]+;( U;)? CPU.+Mac OS X/); //ios终端
    let result = isAndroid || isiOS;
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