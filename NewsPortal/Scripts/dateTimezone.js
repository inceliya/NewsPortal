console.log('jjj');
window.onload = function () {
    console.log('jjj');
    document.querySelectorAll(".dateUtc").forEach(i => {
        console.log('jjj');
        const json = JSON.parse(i.innerHTML);
        let date = new Date(json.year, json.month, json.day, json.hour, json.minute);
        date.setTime(date.getTime() - new Date().getTimezoneOffset() * 60 * 1000);
        i.innerHTML = fillZero(date.getDay()) + "." + fillZero(date.getMonth()) + "." + fillZero(date.getFullYear(), 4) + " " + fillZero(date.getHours()) + ":" + fillZero(date.getMinutes());;
    })
}
function fillZero(input, size = 2) {
   let i = input.toString();
    while (i.length < size) {
        i = "0" + i;
    }
    return i;
}