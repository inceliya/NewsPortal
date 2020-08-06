document.addEventListener("DOMContentLoaded", function () {
    console.log(5);
    let url = loader.dataset.request.substring(0, loader.dataset.request.length - 1);
    if (loader.dataset.view.toString().toLowerCase() == "index") {
        let sort = loader.dataset.sort;
        //let reverseBool = loader.dataset.reverse == "true";
        let reverseBool = loader.dataset.reverse;
        if (reverseBool == "False")
            reverseBool = false;
        else
            reverseBool = true;

        switch (sort) {
            case "date":
                if (reverseBool)
                    dropdownMenu1.innerHTML = $(lang).data('new-first');
                else
                    dropdownMenu1.innerHTML = $(lang).data('old-first');
                break;
            case "title":
                if (reverseBool)
                    dropdownMenu1.innerHTML = $(lang).data('title-z-a');
                else
                    dropdownMenu1.innerHTML = $(lang).data('title-a-z');
                break;
            case "description":
                if (reverseBool)
                    dropdownMenu1.innerHTML = $(lang).data('desc-z-a');
                else
                    dropdownMenu1.innerHTML = $(lang).data('desc-a-z');
                break;
        }
        let caret = document.createElement("span");
        caret.classList.add("caret");
        dropdownMenu1.append(caret);
    }
    $(search).keypress((event) => {
        if (event.which == 13) {
            event.preventDefault();
            if (search.value.toString())
                location.href = url + search.value;
        };
    })
    $(search).change(() => {
        if (search.value.toString())
            location.href = url + search.value;
    })


})