function activeCarousel(obj) {
    const next = document.querySelector(obj.next) || "";
    const prev = document.querySelector(obj.prev) || "";
    const items = document.querySelectorAll(obj.carousel + ".carousel-container .carousel-slider .carousel-item");
    items[0].style.zIndex = 1;
    const size = items.length;
    let index = 1;
    let currentItem;
    let canSlide = true;
    if (obj.interval) setInterval(() => performSlide(1), obj.interval * 1000);
    next.onclick = () => performSlide(1);
    prev.onclick = () => performSlide(-1);
    function performSlide(dir) {
        if (canSlide) {
            let item;
            let dirIndex;
            let curIndex;
            if (dir > 0) {
                if (index == size) index = 0;
                dirIndex = index;
                curIndex = index - 1 < 0 ? size - 1 : index - 1;
            } else if (dir < 0) {
                if (index - 1 < 0) index = size;
                dirIndex = index - 2 < 0 ? size - 1 : index - 2;
                curIndex = index - 1;
            }
            item = items[dirIndex];
            currentItem = items[curIndex];
            const currentX = removeFromString(currentItem.style.transform, ["translateX", "(", "%)"]);
            item.style.transition = "transform .00000000000000000000000001s";
            item.style.transform = `translateX(${(currentX + 100) * dir}%)`;
            slide(item, dir);
            canSlide = false;
        }
    }
    function slide(itemSlide, dir) {
        itemSlide.addEventListener("transitionend", () => {
            items.forEach(item => {
                item.style.zIndex = 0;
                if (item == currentItem) item.style.zIndex = 1;
            });
            const currentX = removeFromString(currentItem.style.transform, ["translateX", "(", "%)"]);
            itemSlide.style.zIndex = index;
            if (index == 1) items[0].style.zIndex = size + 1;
            if (dir < 0 && index == 0) items[size - 1].style.zIndex = size + 1;
            itemSlide.style.transition = `transform ${obj.time}s ${obj.anim}`;
            itemSlide.style.transform = `translateX(${currentX * dir}%)`;
            if (!obj.overlap) {
                currentItem.style.transition = `transform ${obj.time}s ${obj.anim}`;
                currentItem.style.transform = `translateX(${(currentX - 100) * dir}%)`;
            }
            itemSlide.addEventListener("transitionend", () => {
                canSlide = true;
                currentItem.style.transition = "none";
                currentItem.style.transform = "translateX(0)";
            }, { once: true });
        }, { once: true });
        index = dir > 0 ? index += 1 : index -= 1;
    }
}

function getElementY(element, elementOffset) {
    let bodyRect = document.body.getBoundingClientRect(),
        el = typeof (element) === "string" ? document.querySelector(element) : element,
        elemRect = el.getBoundingClientRect(),
        offset = (elemRect.top - bodyRect.top) + elementOffset;
    return offset;
}

function getMousePosInElementCenter(el) {
    const rect = el.getBoundingClientRect();
    const x = event.clientX - rect.left - rect.width / 2;
    const y = event.clientY - rect.top - rect.height / 2;
    return { x, y };
}

function getMousePosInElement(el) {
    const rect = el.getBoundingClientRect();
    const x = event.clientX - rect.left;
    const y = event.clientY - rect.top;
    return { x, y };
}

function lerp(p, p1, v) {
    return (p1 - p) * v;
}

function normalize(value, max) {
    return value / max;
}

var scrolling = false;
function scrollToElement(el, offset) {
    if (scrolling) return;
    let index = window.scrollY;
    const goal = el === "" ? -10 : getElementY(el, offset);
    let interval = setInterval(() => {
        index += lerp(index, goal, .0175);
        window.scrollTo(0, index);
        if (Math.abs(goal - index) < 10) {
            clearInterval(interval)
            scrolling = false;
        }
        else scrolling = true;
    }, 1);
}


/*function zoomImg(e) {
    e.style.overflow = "hidden";
    const pos = getMousePosInElementCenter(e);
    const x = pos.x;
    const y = pos.y;
    const img = e.children[0];
    img.style.cursor = "crosshair";
    img.style.transform = `translate(${-x}px, ${-y}px) scale(2)`;
    img.style.transition = "all .1s ease-out"
}

function resetZoom(e) {
    const img = e.children[0];
    img.style.transition = "all .2s cubic-bezier(.01,.79,.39,.98)"
    img.style.opacity = 1;
    img.style.transform = "scale(1)";
    img.style.left = 0;
    img.style.top = 0;
}*/

function loginContent(index) {
    const rowLogin = document.querySelector(".row-login");
    rowLogin.style.transform = `translateX(-${index * 100}%)`;
    return false;
}

setExplosionAnim(".sub-btn");
function setExplosionAnim(obj) {
    const btn = document.querySelectorAll(obj);
    btn.forEach(btn => {
        btn.addEventListener("mousedown", () => {
            btn.style.setProperty("--btn-left", 100 * getMousePosInElement(btn).x / btn.clientWidth + "%");
            btn.classList.add("explosion-anim");
            setTimeout(() => btn.classList.add("explosion-anim-end"), 0);
        });

        btn.addEventListener("transitionend", () => {
            btn.classList.remove("explosion-anim");
            btn.classList.remove("explosion-anim-end");
        });
    });
}
activeNav();
activeAsideCart();

function activeProfileUI() {
    const profileOptionsContainer = document.querySelectorAll(".options-container");
    profileOptionsContainer.forEach(pop => {
        const itemWidth = 100 / pop.childElementCount <= 33.3333 ? 33.3333 : 100 / pop.childElementCount;
        for (let i = 0; i < pop.childElementCount; i++) {
            const item = pop.children[i];
            const content = item.querySelector(".content");
            const backBtn = content.querySelector(".back-to-data-btn");
            const contentSlider = content.querySelector(".content-slider");

            const selectServiceContainer = content.querySelector(".package-select-service-container");
            if (selectServiceContainer !== null) {
                const tableContent = selectServiceContainer.querySelector(".table-content");
                const serviceItems = tableContent.querySelectorAll(".data-item");
                const inputServices = pop.querySelector("[data-id=services-input]");
                let selectedServices = tableContent.getAttribute("data-selected-services").split(",");

                var observer = new MutationObserver(function (mutations) {
                    mutations.forEach(function (mutation) {
                        if (mutation.type == "attributes" & mutation.attributeName == "data-selected-services") {
                            selectedServices = tableContent.getAttribute("data-selected-services").split(",");
                            const servicesName = [];
                            serviceItems.forEach(s => {
                                const servId = s.getAttribute("data-id");
                                for (let j = 0; j < selectedServices.length; j++) {
                                    const ss = selectedServices[j];
                                    if (ss !== servId & s.getAttribute("data-selected") === "true") s.setAttribute("data-selected", false);
                                }
                                for (let j = 0; j < selectedServices.length; j++) {
                                    const ss = selectedServices[j];
                                    if (ss === servId) {
                                        s.setAttribute("data-selected", true)
                                        servicesName.push(s.getAttribute("data-name"));
                                    };
                                }
                                inputServices.innerHTML = servicesName.join(", ");
                                if (s.getAttribute("data-selected") === "true") {
                                    s.children[0].classList.add("main-bg-green");
                                } else if (s.getAttribute("data-selected") === "false") {
                                    s.children[0].classList.remove("main-bg-green");
                                }
                            });
                        }
                    });
                });
                observer.observe(tableContent, { attributes: true });

                serviceItems.forEach(s => {
                    const serviceId = s.getAttribute("data-id");
                    s.onclick = () => {
                        const selected = s.getAttribute("data-selected");
                        if (selected === "false") {
                            const curSelected = tableContent.getAttribute("data-selected-services").split(",");
                            curSelected.push(serviceId)
                            tableContent.setAttribute("data-selected-services", curSelected);
                        } else {
                            let curSelected = tableContent.getAttribute("data-selected-services").split(",");
                            curSelected = curSelected.filter(x => x !== serviceId);
                            tableContent.setAttribute("data-selected-services", curSelected);
                        }
                    }
                })
            }

            if (contentSlider !== null) {
                const dataItems = contentSlider.querySelector(".table-content").querySelectorAll(".data-item");
                contentSlider.style.transition = "transform .7s var(--main-bezier)";
                dataItems.forEach(item => item.onclick = () => {
                    generateNewSignature().then(v => {
                        const objs = [{ obj: "Customer", action: "/Customer/Get", params: { id: item.getAttribute("data-id") }, onSuccess: "customerDataTaken", onFailure: "customerDataFail" },
                            { obj: "Employee", action: "/Employee/Get", params: { id: item.getAttribute("data-id") }, onSuccess: "employeeDataTaken", onFailure: "employeeDataFail" },
                            { obj: "Package", action: "/Package/Get", params: { id: item.getAttribute("data-id") }, onSuccess: "packageDataTaken", onFailure: "packageDataFail" },
                            { obj: "Service", action: "/Service/Get", params: { id: item.getAttribute("data-id") }, onSuccess: "serviceDataTaken", onFailure: "serviceDataFail" },
                            { obj: "Schedule", action: "/Schedule/Get", params: { id: item.getAttribute("data-id") }, onSuccess: "scheduleDataTaken", onFailure: "scheduleDataFail" },]

                        objs.forEach(o => {
                            if (item.getAttribute("data-table-of") === o.obj) sendRequest("post", o.action, o.params, {
                                Loader: ".main-loader", OnSuccess: o.onSuccess, OnFailure: o.onFailure,
                                ContentHeaders: [{ Key: "Authorization", Value: `Bearer ${v}` }]
                            });
                        });
                    });
                    contentSlider.style.transform = "translateX(-100%)";
                });
                backBtn.onclick = () => contentSlider.style.transform = "translateX(0)";
            }

            item.style.minWidth = itemWidth + "%";
            item.style.maxWidth = itemWidth + "%";
            item.style.transition = `transform .7s var(--main-bezier) ${i / 8}s, 
            min-width .7s var(--main-bezier), max-width .7s var(--main-bezier)`;
            const openBtn = item.querySelector(".open");
            const closeBtn = item.querySelector(".close");
            closeBtn.style.transition = "opacity 2s var(--main-bezier), visibility 2s var(--main-bezier)";
            addTransitionProperty(openBtn, "opacity 1s");
            addTransitionProperty(openBtn, "visibility 1s");
            content.style.transition = "opacity .75s var(--main-bezier), visibility .75s var(--main-bezier)";
            openBtn.onclick = () => {
                item.style.maxWidth = "100%";
                item.style.minWidth = "100%";
                item.style.zIndex = 1;
                closeBtn.style.visibility = "visible";
                closeBtn.style.opacity = 1;
                openBtn.style.visibility = "hidden";
                openBtn.style.opacity = 0;
                content.style.visibility = "visible";
                content.style.opacity = 1;
                for (let i = 0; i < pop.childElementCount; i++) {
                    const aItem = pop.children[i];
                    if (aItem !== item) {
                        aItem.style.zIndex = 0;
                        aItem.style.maxWidth = 0;
                        aItem.style.minWidth = 0;
                        if (window.innerWidth > 992) {
                            const openBtn = aItem.querySelector(".open");
                            openBtn.style.opacity = 0;
                        }
                    }
                }
            }
            closeBtn.onclick = () => {
                closeBtn.style.opacity = 0;
                closeBtn.style.visibility = "hidden";
                openBtn.style.opacity = 1;
                openBtn.style.visibility = "visible";
                content.style.opacity = 0;
                content.style.visibility = "hidden";
                for (let i = 0; i < pop.childElementCount; i++) {
                    const item = pop.children[i];
                    item.style.maxWidth = itemWidth + "%";
                    item.style.minWidth = itemWidth + "%";
                    const openBtn = item.querySelector(".open");
                    openBtn.style.opacity = 1;
                }
            }
        }
    });

    //window.onload = () => {
    profileOptionsContainer.forEach(pop => {
        for (let i = 0; i < pop.childElementCount; i++) {
            const item = pop.children[i];
            item.style.transform = "translateY(0)";
        }
    });
    //}
}


const toastContainer = document.querySelector(".toast-container");
toastContainer.insertAdjacentHTML("afterbegin", "<div class='nav-height m-b-2'></div>");
function createToast(text, type) {
    if (text !== "" & text !== null & text !== undefined) {
        const toastContainer = document.querySelector(".toast-container");
        const htmlToast = `<toast><span>${text}</span><i class="fas fa-times"></i></toast>`;
        toastContainer.insertAdjacentHTML("beforeend", htmlToast);
        const toast = toastContainer.children[toastContainer.childElementCount - 1];
        const types = ["var(--main-green)", "var(--main-yellow)", "var(--main-red)"];
        toast.style.backgroundColor = types[type];
        const btn = toast.querySelector("i");
        if (toastContainer.childElementCount > 2) toast.style.marginTop = ".5rem";
        btn.onclick = () => removeToast();
        setTimeout(() => removeToast(), 5000);

        function removeToast() {
            toast.style.animation = "none";
            toast.style.transition = "opacity .5s var(--main-bezier)";
            setTimeout(() => {
                toast.style.opacity = 0;
                setTimeout(() => toast.remove(), 500);
            }, 0);
        }
    }
}

const mainFooter = document.querySelector(".main-footer");
const deleteFooter = document.querySelector("remove-footer");
if (deleteFooter) mainFooter.remove();

activeRating();
function activeRating() {
    const textViewArray = ["Detestei", "Não Gostei", "Razoável", "Gostei", "Amei"];
    const starContainer = document.querySelectorAll(".rate-container");
    let textVoted;
    starContainer.forEach(rc => {
        const starItems = rc.querySelectorAll(".star-item");
        if (rc.getAttribute("data-stars-visualization") !== "") {
            const textView = rc.querySelector(".text-view");
            const inputValue = rc.querySelector("input");
            rc.setAttribute("data-voted", "false");
            for (let i = 0; i < starItems.length; i++) {
                let factor = 0;
                const si = starItems[i];
                si.setAttribute("data-evaluation-note", i + 1);
                si.setAttribute("data-selected", "false");
                si.onmouseenter = () => {
                    textView.innerHTML = textViewArray[i];
                    for (let j = 0; j <= i; j++) {
                        const a = starItems[j];
                        const star = a.querySelector(".star");
                        star.style.filter = "brightness(30%)"; star.style.backgroundColor = "rgb(255, 230, 0)";
                    }
                }
                si.onmouseleave = () => {
                    const voted = rc.getAttribute("data-voted");
                    const t = textVoted === undefined ? textViewArray[i] : textVoted;
                    textView.innerHTML = t;
                    if (voted === "false") textView.innerHTML = "Não votado ainda...";
                    for (let j = 0; j <= i; j++) {
                        const a = starItems[j];
                        const star = a.querySelector(".star");
                        star.style.backgroundColor = star.style.backgroundColor = "rgba(0, 0, 0, .6)";
                    }
                }

                si.onclick = () => {
                    factor = 0;
                    rc.setAttribute("data-voted", "true");
                    rc.setAttribute("data-rating-value", si.getAttribute("data-evaluation-note"));
                    if (inputValue !== null) inputValue.value = si.getAttribute("data-evaluation-note");
                    textVoted = textViewArray[i];
                    for (let j = 0; j <= i; j++) {
                        const a = starItems[j];
                        const starChild = a.querySelector(".star-child");
                        starChild.style.transition = `transform .5s var(--main-bezier) ${(j + 1) / 2 / 10}s`;
                        starChild.style.transform = "scale(1)";
                        starItems.forEach(a => a.setAttribute("data-selected", "false"));
                        if (i === j) si.setAttribute("data-selected", "true");
                    }
                    for (let j = starItems.length - 1; j > i; j--) {
                        const a = starItems[j];
                        const starChild = a.querySelector(".star-child");
                        starChild.style.transition = `transform .5s var(--main-bezier) ${(factor + 1) / 2 / 10}s`;
                        starChild.style.transform = "scale(0)";
                        factor++;
                    }
                }
            }
        } else {
            const ratingView = rc.querySelector(".rating-view");
            const ratingValue = rc.getAttribute("data-rating");
            if (ratingView !== null) {
                ratingView.style.color = "gray";
                ratingView.innerHTML = ratingValue === "" ? "Sem avaliação" : ratingValue;
            }
            if (ratingValue !== "") {
                const entireRV = Math.floor(ratingValue);
                const ei = entireRV === 5 ? 0 : 1;
                for (let i = 0; i < entireRV + ei; i++) {
                    if (starItems[i] !== undefined) {
                        const star = starItems[i].children[0];
                        const width = i === entireRV ? `${(ratingValue - entireRV) * 100}%` : "100%";
                        star.style.setProperty("--data-star-value-width", width);
                    }
                }
            }
        }

    });
}

activeArrowPointer();
function activeArrowPointer() {
    const arrow = document.querySelector(".arrow-pointer");
    if (arrow !== null) {
        setTimeout(() => {
            arrow.style.opacity = 0;
            arrow.style.visibility = "hidden";
        }, 10000);
    }
}

activeSmoothButton();
function activeSmoothButton() {
    const btnsToEffects = document.querySelectorAll("[data-follow-smoothly]");
    btnsToEffects.forEach(b => {
        b.onmousemove = () => followSmoothly(b);
        const c = b.children[0];
        c.classList.add("transition");
        b.onmouseleave = () => c.style.transform = `translate(0, 0)`;
    });
}

activeDataColorChange();
function activeDataColorChange() {
    const dataAnimTo = document.querySelectorAll("[data-anim-to]");
    dataAnimTo.forEach(d => {
        const child = d.children[0];
        const b = getComputedStyle(d).backgroundColor;
        const color = d.getAttribute("data-anim-to");
        addTransitionProperty(d, "background-color .2s");
        addTransitionProperty(d, "color .2s");

        d.addEventListener("mouseover", () => {
            d.style.backgroundColor = color;
            if (child != undefined) {
                child.style.color = color;
                child.style.filter = "invert(1)";
            }
        });
        d.addEventListener("mouseout", () => {
            d.style.backgroundColor = b;
            if (child != undefined) child.style.color = b;
        });
    });
}

function addTransitionProperty(element, transition, delay) {
    delay = delay !== undefined ? delay : "0s";
    const t = getComputedStyle(element).transition;
    const anim = `${transition} var(--main-bezier) ${delay}`;
    element.style.transition = t !== "all 0s ease 0s" ? t + ", " + anim : anim;
}

activeDataZoom();
function activeDataZoom() {
    const dataZoom = document.querySelectorAll("[data-zoom]");
    dataZoom.forEach(d => {
        const zoom = d.getAttribute("data-zoom").split(",");
        addTransitionProperty(d, `transform ${zoom[1]}`);
        d.addEventListener("mouseover", () => d.style.transform = `scale(${zoom[0]})`);
        d.addEventListener("mouseout", () => d.style.transform = `scale(1)`);
    });
}

activePriceRange();
function activePriceRange() {
    const priceInput = document.querySelectorAll(".price-input");
    priceInput.forEach(p => {
        const btns = p.querySelectorAll("button");
        const input = p.querySelector("input");
        for (let i = 0; i < btns.length; i++) {
            const b = btns[i];
            const fac = i === 0 ? -1 : 1;
            b.onclick = () => {
                const value = Number(input.value) + 50 * fac;
                const max = Number(input.getAttribute("max"));
                const min = Number(input.getAttribute("min"));
                input.value = value;
                if (value > max) input.value = max;
                else if (value < min) input.value = min;
            };
        }
    });
}

activeInputManager();
function activeInputManager() {
    const inputs = document.querySelectorAll(".customized-input");
    inputs.forEach(inputDiv => {
        const input = inputDiv.querySelector("input") || inputDiv.querySelector("textarea");
        if (input !== null) {
            input.setAttribute("data-status", "idle");
            const editBtn = inputDiv.querySelector(".edit-btn");
            const saveBtn = inputDiv.querySelector(".save-btn");
            if (editBtn !== null && saveBtn !== null) {
                editBtn.addEventListener("click", () => {
                    input.setAttribute("data-status", "editing");
                    input.removeAttribute("disabled");
                    editBtn.classList.add("main-red");
                });
                saveBtn.addEventListener("click", () => {
                    if (input.getAttribute("data-status") === "editing") {
                        input.setAttribute("disabled", "true");
                        editBtn.classList.remove("main-red");
                    }
                });
            }
        }
    });
}

activeCheckbox();
function activeCheckbox() {
    const customizedCheckbox = document.querySelectorAll(".customized-checkbox");
    customizedCheckbox.forEach(c => {
        const input = c.querySelector("input");
        input.value = false;
        input.classList.add("cursor-pointer");
        c.classList.add("cursor-pointer");
        c.addEventListener("click", () => {
            let checked = input.value;
            if (checked === "true") {
                input.value = false;
                input.style.transform = "scale(0)";
            } else if (checked === "false") {
                input.value = true;
                input.style.transform = "scale(1)";
            }
        });
    });
}

activeDropDown();
function activeDropDown() {
    const customizedDropdown = document.querySelectorAll(".customized-dropdown");
    customizedDropdown.forEach(c => {
        const input = c.querySelector(".dropdown-select-item");
        const btn = c.querySelector(".open-dropdown");
        const content = c.querySelector(".dropdown-content");
        input.setAttribute("data-open", false);
        const icon = btn.querySelector("i");
        icon.style.transition = "transform .2s var(--main-bezier)";
        content.style.transition = "height .4s var(--main-bezier)";

        const items = content.querySelectorAll("label");
        const option = input.getAttribute("data-option");
        const radios = content.querySelectorAll("input");

        input.innerHTML = items[0].innerHTML;
        input.setAttribute("data-option", items[0].getAttribute("data-option"));
        radios[0].checked = true;

        for (let i = 0; i < radios.length; i++) {
            const radio = radios[i];
            if (radio.value == option) {
                input.innerHTML = items[i].innerHTML;
                radio.checked = true;
                break;
            }
        }

        var observer = new MutationObserver(function (mutations) {
            mutations.forEach(function (mutation) {
                if (mutation.type == "attributes" & mutation.attributeName == "data-option") {
                    const newOption = input.getAttribute("data-option");
                    input.setAttribute("data-open", false);
                    content.style.height = 0;
                    icon.style.transform = "rotateX(0)";
                    items.forEach(i => { if (i.getAttribute("data-option") === newOption) input.innerHTML = i.innerHTML; });
                }
            });
        });

        observer.observe(input, {
            attributes: true 
        });

        items.forEach(item => {
            item.onclick = () => {
                input.setAttribute("data-open", false);
                content.style.height = 0;
                icon.style.transform = "rotateX(0)";
                input.setAttribute("data-option", item.getAttribute("data-option"));
                input.innerHTML = item.innerHTML;
            }
        });
        const max = 4;
        const factor = items.length > max ? max : items.length;
        const fixedHeight = `${factor}00%`;
        content.style.overflowY = items.length <= max ? "hidden" : "scroll";
        content.style.maxHeight = fixedHeight;
        btn.onclick = () => {
            let isOpen = input.getAttribute("data-open");
            if (isOpen === "true") {
                icon.style.transform = "rotateX(0)";
                input.setAttribute("data-open", false);
                content.style.height = 0;
            } else {
                icon.style.transform = "rotateX(180deg)";
                input.setAttribute("data-open", true);
                content.style.height = fixedHeight;
            }
        }
    });
}

activeBooleanInput();
function activeBooleanInput() {
    const customizedBooleanType = document.querySelectorAll(".boolean-type");
    customizedBooleanType.forEach(b => {
        const btns = b.children;
        btns[0].addEventListener("click", () => b.setAttribute("data-bool", false));
        btns[1].addEventListener("click", () => b.setAttribute("data-bool", true));
        var observer = new MutationObserver(function (mutations) {
            mutations.forEach(function (mutation) {
                if (mutation.type == "attributes" & mutation.attributeName == "data-bool") {
                    const bool = b.getAttribute("data-bool");
                    if (bool === "true") {
                        btns[0].removeAttribute("selected");
                        btns[1].setAttribute("selected", "");
                        btns[0].style.filter = "";
                        btns[1].style.filter = "invert(1)";
                    }
                    else {
                        btns[0].setAttribute("selected", "");
                        btns[1].removeAttribute("selected");
                        btns[0].style.filter = "invert(1)";
                        btns[1].style.filter = "";
                    }
                }
            });
        });

        observer.observe(b, {
            attributes: true
        });
    });
}

function followSmoothly(pointerArea) {
    const item = pointerArea.children[0];
    const pos = getMousePosInElementCenter(pointerArea);
    const area = pointerArea.getBoundingClientRect();
    const content = item.getBoundingClientRect();
    const x = normalize(pos.x, area.width);
    const y = normalize(pos.y, area.height);
    const maxX = area.width - content.width;
    const maxY = area.height - content.height;
    item.style.transform = `translate(${x * maxX}px, ${y * maxY}px)`;
}

function activeNav() {
    const menuBtn = document.querySelector(".menu-btn");
    menuBtn.onclick = () => {
        //const text = document.querySelector(".menu-btn-text");
        const text = document.querySelector(".menu-text");
        const menuIcon = document.querySelector(".nav-menu-icon");
        const icons = document.querySelectorAll(".nav-btn-icon");
        const menuContainer = document.querySelector(".menu-container");
        const isOpen = menuContainer.getAttribute("data-open");
        menuContainer.style.transition = "transform 1s cubic-bezier(0,.81,.11,1)";
        if (isOpen === "true") {
            menuContainer.setAttribute("data-open", false)
            menuContainer.style.transform = "translateX(0)";
            icons.forEach(i => {
                i.style.transition = "color .9s ease-out, transform 1s cubic-bezier(.19,.62,.4,.77)";
                i.style.color = "white";
            });
            text.style.transition = "color .2s ease-out";
            text.style.color = "white";
            menuIcon.classList.remove("fa-times");
            menuIcon.classList.add("fa-bars");
        } else {
            menuContainer.setAttribute("data-open", true);
            menuContainer.style.transform = "translateX(-100%)";
            icons.forEach(i => {
                i.style.transition = "color .2s ease-out, transform 1s cubic-bezier(.19,.62,.4,.77)";
                i.style.color = "black";
            });
            text.style.transition = "color .9s";
            text.style.color = "black";
            menuIcon.classList.remove("fa-bars");
            menuIcon.classList.add("fa-times");
        }
    }
}

function showElementOnScroll(obj, method, method2) {
    if (obj == undefined) return;
    const y = window.scrollY;
    let o = obj.off ? obj.off : 0;
    o = obj.final ? o - window.innerHeight : o;
    const offset = getElementY(obj.offset, o);
    if (y >= offset) method();
    if (method2 && y < offset)
        method2();
}

function overtake(obj, method, method2) {
    const el1 = typeof (obj.el) === "string" ? document.querySelector(obj.el) : obj.el;
    const el2 = typeof (obj.goal) === "string" ? document.querySelector(obj.goal) : obj.goal;
    const el1Y = getElementY(el1, 0);
    let el2Y = obj.final ? getElementY(el2, 0) + el2.getBoundingClientRect().height : getElementY(el2, 0);
    el2Y = obj.off ? el2Y + obj.off : el2Y;
    if (el1Y > el2Y) method(el1);
    if (method2 && el1Y < el2Y) method2(el1);
}

function activeLoginForm() {
    const translates = [".translate-0", ".translate-1", ".translate-2"];
    translates.forEach(t => {
        const tArray = document.querySelectorAll(t);
        for (let i = 0; i < tArray.length; i++) {
            const item = tArray[i];
            item.onclick = () => loginContent(t.charAt(t.length - 1));
        }
    });
}

function navEffect(fillBlackOnElement) {
    const mainNav = document.querySelector(".nav");
    const height = document.querySelector(fillBlackOnElement).clientHeight;
    const offsetY = normalize(window.scrollY, height);
    mainNav.style.backgroundColor = `rgba(0, 0, 0, ${offsetY})`;
}

function homeScrollEffects() {
    document.querySelector(".nav").style.backgroundColor = `transparent`;
    window.addEventListener("scroll", () => {
        if (window.innerWidth > 992 && window.scrollY < window.innerHeight) {
            const container = document.querySelector(".main-container");
            const offsetY = normalize(window.scrollY, container.clientHeight);
            const t = offsetY * 150;
            container.style.transform = `translateY(${-t}px)`;
        }

        navEffect(".main-container");
        callAllEffects();
        activePromisesAnim();
        toggleAsideColors();
        fillAsideCircles();
    });
}

function callAllEffects() {
    showElementOnScroll({ offset: ".box" }, () => {
        document.querySelector(".box2-text").style.opacity = 1;
        document.querySelector(".box2").style.opacity = 1;
    });
    showElementOnScroll({ final: true, offset: ".service-h1" }, () => {
        document.querySelector(".service-h1").style.transform = "translateX(0)";
        document.querySelector(".service-h1").style.right = 0;
        document.querySelector(".service-p").style.transform = "translateX(0)";
        document.querySelector(".service-p").style.left = 0;
    });
    showElementOnScroll({ final: true, offset: ".package-h1" }, () => {
        document.querySelector(".package-h1").style.transform = "translateX(0)";
        document.querySelector(".package-h1").style.right = 0;
        document.querySelector(".package-p").style.transform = "translateX(0)";
        document.querySelector(".package-p").style.left = 0;
    });
    showElementOnScroll({ final: true, offset: ".contact-h1" }, () => {
        document.querySelector(".contact-h1").style.transform = "translateX(0)";
        document.querySelector(".contact-h1").style.right = 0;
        document.querySelector(".contact-p").style.transform = "translateX(0)";
        document.querySelector(".contact-p").style.left = 0;
    });

    effectOffsetBottom({ item: ".box", offset: ".box", factor: 300, x: -20 });
    effectOffsetBottom({ item: ".box-text", offset: ".box-text", factor: 200, x: 20 });
    effectOffsetBottom({ item: ".box2", offset: ".main-carousel", factor: 170, x: 20 });
    effectOffsetBottom({ item: ".box2-text", offset: ".main-carousel", factor: 100, x: -20 });
    effectOffsetBottom({ item: ".contact-fieldset", offset: ".contact-fieldset", factor: 100, x: 0 });

}

function activePromisesAnim() {
    const promisesContent = document.querySelectorAll(".promise-content");
    for (let i = 0; i < promisesContent.length; i++) {
        let p = promisesContent[i];
        showElementOnScroll({ offset: ".box2-text" }, () => {
            p.style.transition = `transform 1s cubic-bezier(.69,0,.26,.99), opacity ${i}s ease-out`;
            p.style.opacity = 1;
            p.style.transform = "translateY(0)";
        });
    }
}

function activeAside() {
    const asideCircle = document.querySelectorAll(".menu-circle");
    const asideReferences = [{ el: "", off: 00 }, { el: ".about-container", off: -400 },
    { el: ".middle-container", off: -60 }, { el: ".services-container", off: -60 },
    { el: ".package-container", off: -60 }, { el: ".contact-container", off: -60 }];
    for (let i = 0; i < asideCircle.length; i++) {
        const c = asideCircle[i];
        const cc = asideCircle[i].children[0];
        cc.style.transform = "scale(0)";
        cc.style.transition = "transform 1s cubic-bezier(0,.69,.32,1)";
        c.onclick = () => {
            scrollToElement(asideReferences[i].el, asideReferences[i].off);
        }
    }
}

activeFormValidate();
function activeFormValidate() {
    const emailRegex = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$/;
    const strongPasswordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])/;
    const validations = [
        { val: "data-val-email", valFunc: (value, valueSender) => emailRegex.test(value), failure: (fieldName, vs) => `${fieldName} / Inválido` },
        { val: "data-val-max-length", valFunc: (value, valueSender) => value.length < valueSender, failure: (fieldName, vs) => `${fieldName} / Máximo de ${vs} caracteres` },
        { val: "data-val-min-length", valFunc: (value, valueSender) => value.length > valueSender, failure: (fieldName, vs) => `${fieldName} / Mínimo de ${vs} caracteres` },
        { val: "data-val-regex", valFunc: (value, valueSender) => valueSender.test(value), failure: (fieldName, vs) => `${fieldName}` },
        { val: "data-val-password", valFunc: (value, valueSender) => strongPasswordRegex.test(value), failure: (fieldName, vs) => `${fieldName} / Deve conter letras maiúsculas + letras minúsculas + números` },
        { val: "data-val-equalto", valFunc: (value, valueSender) => document.querySelector(valueSender).value === value, failure: (fieldName, vs) => `Senhas Diferentes!` }
    ];

    const forms = document.querySelectorAll("[data-form-validate]");
    forms.forEach(form => {
        form.setAttribute("data-form-is-valid", false);
        form.addEventListener("submit", evt => {
            const inputs = form.querySelectorAll("input");
            inputs.forEach(input => {
                let isValid = false;
                input.setAttribute("data-val", isValid);
                let storeValData = [];
                validations.forEach(v => {
                    if (input.hasAttribute(v.val)) {
                        const valueSender = input.getAttribute(v.val);
                        const fieldName = input.getAttribute("data-val-field");
                        storeValData.push({ valName: v.val, valFailure: v.failure(fieldName, valueSender), valBool: v.valFunc(input.value, valueSender) });
                    }
                });
                storeValData = storeValData.filter(svd => !svd.valBool);
                if (storeValData.length !== 0) storeValData.forEach(svd => createToast(svd.valFailure, 1));
                else isValid = true;
                input.setAttribute("data-val", isValid);
            });
            let formIsValid = true;
            for (let i = 0; i < inputs.length; i++) {
                const input = inputs[i];
                if (input.getAttribute("data-val") === "false") {
                    formIsValid = false;
                    break;
                }
            }
            form.setAttribute("data-form-is-valid", formIsValid);
            evt.preventDefault();
        });
    });
}

activeRequestByForm();
function activeRequestByForm() {
    const forms = document.querySelectorAll("[data-form-request-sender]");
    forms.forEach((f) => {
        f.addEventListener("submit", evt => {
            const method = f.getAttribute("method"), action = f.getAttribute("action");
            const onSuccess = f.getAttribute("data-on-success"), onFailure = f.getAttribute("data-on-failure");
            const formData = new FormData(f);
            const params = f.getAttribute("data-form-params");
            const loader = f.getAttribute("data-loader");
            if (params) JSON.parse(params).forEach(data => {
                formData.append(data[0], data[1]);
            });
            const formIsValid = f.getAttribute("data-form-is-valid");
            if (f.hasAttribute("data-if-is-signed") & formIsValid === "true" | f.hasAttribute("data-if-is-signed") & formIsValid === null) {
                if (isSigned()) {
                    generateNewSignature().then(v => {
                        sendRequest(method, action, formData, {
                            Loader: loader,
                            OnSuccess: onSuccess,
                            OnFailure: onFailure,
                            formRequest: true,
                            ContentHeaders: [{ Key: "Authorization", Value: `Bearer ${v}` }]
                        });
                    });
                }
                else createToast("Você deve estar logado!", 2);
            } else {
                if (f.getAttribute("data-form-is-valid") === "true") {
                    generateNewSignature().then(v => {
                        sendRequest(method, action, formData, {
                            Loader: loader,
                            OnSuccess: onSuccess,
                            OnFailure: onFailure,
                            formRequest: true,
                            ContentHeaders: [{ Key: "Authorization", Value: `Bearer ${v}` }]
                        });
                    });
                    //sendRequest(method, action, formData, { Loader: loader, OnSuccess: onSuccess, OnFailure: onFailure, formRequest: true });
                }
            }
            evt.preventDefault();
        });
    });
}

activeRequestByElement();
function activeRequestByElement(element) {
    if (element !== null & element !== undefined) activeElementSender(element);
    else {
        const items = document.querySelectorAll("[data-element-request-sender]");
        items.forEach(i => activeElementSender(i));
    }

    function activeElementSender(element) {
        const action = element.getAttribute("data-action");
        const params = element.getAttribute("data-params");
        const method = element.getAttribute("data-method");
        const onSuccess = element.getAttribute("data-on-success"), onFailure = element.getAttribute("data-on-failure");
        const loader = element.getAttribute("data-loader");
        element.addEventListener("click", () => {
            generateNewSignature().then(v => {
                sendRequest(method, action, params, {
                    Loader: loader,
                    OnSuccess: onSuccess,
                    OnFailure: onFailure,
                    ContentHeaders: [{ Key: "Authorization", Value: `Bearer ${v}` }]
                });
            });
            //sendRequest(method, action, params, { Loader: loader, OnSuccess: onSuccess, OnFailure: onFailure });
        });
    }
}

function readImage(element, callback) {
    var files = element.files;
    if (!files.length) {
        alert('Please select a file!');
        return;
    }
    var file = files[0];
    var reader = new FileReader();
    var blob = file.slice(0, file.size);
    reader.readAsDataURL(blob);
    reader.onloadend = function (evt) {
        if (evt.target.readyState == FileReader.DONE) {
            const base64Data = evt.target.result;
            callback(base64Data.split(",")[1]);
        }
    };
}

function readImageFromDatabase(byteArray) {
    return "data:image/png;base64," + new TextDecoder("utf-8").decode(new Uint8Array(byteArray));
}

async function sendRequest(method, url, data, info) {
    await requestSender(method, url, data, info).then(response => { try { eval(info.OnSuccess)(response) } catch{ } }).catch(response => { try { eval(info.OnFailure)(response) } catch{ } });
}

async function requestSender(method, url, data, info) {
    await activeLoader(info.Loader);
    const request = await new Promise((resolve, reject) => {
        const xhr = new XMLHttpRequest();
        xhr.open(method, url, true);
        xhr.responseType = "json";
        if (data) if (info && !info.formRequest) {
            if (typeof (data) === "object") data = JSON.stringify(data);
            xhr.setRequestHeader("Content-Type", "application/json");
        }

        if (info.ContentHeaders) {
            for (let i = 0; i < info.ContentHeaders.length; i++){
                const h = info.ContentHeaders[i];
                xhr.setRequestHeader(h.Key, h.Value);
            }
        }
        /*const token = await generateNewSignature();
        xhr.setRequestHeader("Authorization", `Bearer ${token}`);
        //ContentHeaders: [{ Key: "Authorization", Value: `Bearer ${token}` }]*/

        xhr.onload = () => {
            if (xhr.status >= 400) {
                desactiveLoader(info.Loader);
                reject({ failure: "Alguma coisa deu errado!" });
            }
            resolve(xhr.response)
        };
        xhr.onerror = () => { desactiveLoader(info.Loader); reject({ failure: "Tente Novamente!" }); };
        xhr.send(data);
    });
    await desactiveLoader(info.Loader);
    return request;
}

async function activeLoader(tag) {
    if (tag) {
        loader = document.querySelector(tag);
        loader.style.visibility = "visible";
        loader.style.opacity = 1;
    }
}

async function desactiveLoader(tag, loader) {
    if (tag) {
        loader = document.querySelector(tag);
        loader.style.visibility = "hidden";
        loader.style.opacity = 0;
    }
}

function removeCartItemFromHtml(id) {
    const asideCart = document.querySelector(".aside-cart-container");
    const items = asideCart.querySelectorAll(".cart-item");
    items.forEach(i => {
        if (i.getAttribute("data-id") === id) {
            i.style.transition = "transform 1s var(--main-bezier)";
            i.style.transform = "translateX(-100%)";
            i.addEventListener("transitionend", () => {
                i.remove();
                verifyCartMessage();
            }, { once: true })
        }
    });
}

function verifyCartMessage() {
    let isCartEmpty = false;
    generateNewSignature().then(v => {
        sendRequest("post", "/Cart/IsCartEmpty", { }, {
            OnSuccess: response => {
                isCartEmpty = Boolean(response.isEmpty);
                const cart = document.querySelector(".cart-items");

                if (!isSigned()) addMessage("Você precisa estar logado!");
                else {
                    if (isCartEmpty) addMessage("Carrinho vazio...");
                    else removeMessage();
                }

                function addMessage(txt) {
                    const cartMessage = cart.querySelector(".cart-message");
                    if (!cartMessage) cart.innerHTML += `<div class='cart-message row justify-content-center'><div class='f-3 xxl-w font-jos'>${txt}</div></div>`;
                    else cartMessage.innerHTML = txt;
                }

                function removeMessage() {
                    const cartMessage = cart.querySelector(".cart-message");
                    if (cartMessage) cartMessage.remove();
                }
            },
            ContentHeaders: [{ Key: "Authorization", Value: `Bearer ${v}` }]
        });
    });
}

function getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

function activeAsideCart() {
    const aside = document.querySelector(".aside-cart");
    const asideContainer = document.querySelector(".aside-cart-container");
    const openBtn = document.querySelector(".cart-btn");
    aside.style.transition = "transform .5s var(--main-bezier)";
    asideContainer.style.transition = "background-color .5s var(--main-bezier)";

    asideContainer.addEventListener("click", e => {
        const classList = e.target.classList;
        const isArea = classList.contains("close-aside");
        if (isArea) closeAsideCart();
    });

    openBtn.onclick = () => openAsideCart();

    function openAsideCart() {
        asideContainer.style.zIndex = 999;
        asideContainer.style.backgroundColor = "rgb(0, 0, 0, .8)";
        aside.style.transform = "translateX(0)";
    }

    function closeAsideCart() {
        aside.style.transform = "translateX(-100%)";
        asideContainer.style.backgroundColor = "rgb(0, 0, 0, 0)";
        aside.addEventListener("transitionend", () => asideContainer.style.zIndex = -1, { once: true });
    }
}

function fillAsideCircles() {
    const asideCircle = document.querySelectorAll(".menu-circle");
    const asideReferences = [{ el: ".main-container", off: 0 }, { el: ".about-container", off: -420 },
    { el: ".middle-container", off: -100 }, { el: ".services-container", off: -100 },
    { el: ".package-container", off: -100 }, { el: ".contact-container", off: -100 }];
    for (let i = 0; i < asideCircle.length; i++) {
        const cc = asideCircle[i].children[0];
        cc.style.transform = "scale(0)";
        const cr = asideReferences[i];
        showElementOnScroll({ offset: cr.el, off: cr.off }, el => {
            cc.style.transform = "scale(1.125)";
        });
    }
}

function toggleAsideColors() {
    const asideMenu = document.querySelector(".aside-menu").children[0].children;
    const asideCircle = document.querySelectorAll(".menu-circle");
    for (let i = 0; i < asideMenu.length; i++) {
        const o = asideMenu[i];
        overtake({ el: o, off: -15, goal: ".main-container", final: true }, el => {
            el.style.color = "black";
            asideCircle[i].children[0].style.backgroundColor = "black";
            asideCircle[i].style.borderColor = "black";
            overtake({ el: o, off: -15, goal: ".middle-container" }, el => {
                el.style.color = "white";
                asideCircle[i].children[0].style.backgroundColor = "white";
                asideCircle[i].style.borderColor = "white";
                overtake({ el: o, off: -15, goal: ".services-container" }, el => {
                    el.style.color = "black";
                    asideCircle[i].children[0].style.backgroundColor = "black";
                    asideCircle[i].style.borderColor = "black";
                    overtake({ el: o, off: -15, goal: ".package-container" }, el => {
                        el.style.opacity = 0;
                        el.style.visibility = "hidden";
                        overtake({ el: o, off: -15, goal: ".contact-container" }, el => {
                            el.style.opacity = 1; el.style.visibility = "visible";
                        }, el => { })
                    }, el => { el.style.opacity = 1; el.style.visibility = "visible"; })
                }, el => { el.style.color = "white"; asideCircle[i].children[0].style.backgroundColor = "white"; asideCircle[i].style.borderColor = "white"; });
            }, el => { el.style.color = "black"; asideCircle[i].children[0].style.backgroundColor = "black"; asideCircle[i].style.borderColor = "black"; });
        }, el => { el.style.color = "white"; asideCircle[i].children[0].style.backgroundColor = "white"; asideCircle[i].style.borderColor = "white"; });
    }
}

function activePackageContainer() {
    const packageCards = document.querySelectorAll(".package-card");
    const cards = document.querySelector(".package-container").querySelectorAll(".card");
    const transOut = ["rotateY(20deg)", "scale(.9)", "rotateY(-20deg)"]
    const transIn = ["rotateY(0)", "scale(1)", "rotateY(0)"]
    cards.forEach(card => addTransitionProperty(card, "filter 1s"));
    for (let i = 0; i < packageCards.length; i++) {
        const pc = packageCards[i];
        const line = pc.querySelector(".line");
        line.style.width = 0;
        addTransitionProperty(line, "width .5s");
        addTransitionProperty(pc, "opacity 1s");
        const thisCard = pc.querySelector(".card");
        thisCard.style.transform = transOut[i];
        const cardContent = thisCard.querySelector(".content");
        addTransitionProperty(thisCard, "transform .3s");
        addTransitionProperty(cardContent, "opacity .5s");
        const ch = pc.querySelector(".card-hover");
        pc.style.zIndex = 14;
        addTransitionProperty(ch, `opacity .3s`);

        const itemsToAnim = ch.querySelectorAll(".anim-sequence");
        for (let i = 0; i < itemsToAnim.length; i++) {
            const item = itemsToAnim[i];
            addTransitionProperty(item, `opacity .3s`, `${((i + 1) * 1 + 2) / 10}s`);
            addTransitionProperty(item, `transform .3s`, `${((i + 1) * 1 + 2) / 10}s`);
        }
        ch.onmouseenter = () => {
            pc.style.zIndex = 15;
            line.style.width = "100%";
            ch.style.opacity = 1;
            cardContent.style.opacity = 0;
            thisCard.style.transform = transIn[i];
            itemsToAnim.forEach(item => { item.style.opacity = 1; item.style.transform = "translateY(0)" });
            cards.forEach(card => { if (card !== thisCard) card.style.filter = "brightness(50%)" });
        }
        ch.onmouseleave = () => {
            pc.style.zIndex = 14;
            cardContent.style.opacity = 1;
            ch.style.opacity = 0;
            line.style.width = 0;
            thisCard.style.transform = transOut[i];
            itemsToAnim.forEach(item => { item.style.opacity = 0; item.style.transform = "translateY(50%)" });
            cards.forEach(card => card.style.filter = "brightness(100%)");
        }
    }
}

activePopUps();
function activePopUps() {
    const popUps = document.querySelectorAll("popup");
    function getPopUpById(id) {
        for (let i = 0; i < popUps.length; i++) {
            const pu = popUps[i];
            if (pu.getAttribute("data-id") === id) {
                return pu;
            }
        }
    }

    const openPopUpFromPhoneBtns = document.querySelectorAll("[data-open-pop-up-from-phone]");
    openPopUpFromPhoneBtns.forEach(btn => {
        if (window.innerWidth < 576) {
            const popUpTargetId = btn.getAttribute("data-open-pop-up-from-phone");
            const popUpStyle = getPopUpById(popUpTargetId).style;
            btn.addEventListener("click", () => {
                popUpStyle.transform = "translate(-50%, -50%) scale(1)";
                popUpStyle.opacity = 1;
                popUpStyle.visibility = "visible"
            });
        }
    });

    const openPopUpBtns = document.querySelectorAll("[data-open-pop-up]");
    for (let i = 0; i < openPopUpBtns.length; i++) {
        const openBtn = openPopUpBtns[i];
        const popUpTargetId = openBtn.getAttribute("data-open-pop-up");
        const popUpStyle = getPopUpById(popUpTargetId).style;
        openBtn.addEventListener("click", () => {
            popUpStyle.transform = "translate(-50%, -50%) scale(1)";
            popUpStyle.opacity = 1;
            popUpStyle.visibility = "visible"
        });
    }

    const closePopUpBtns = document.querySelectorAll("[data-close-pop-up]");
    for (let i = 0; i < closePopUpBtns.length; i++) {
        const closeBtn = closePopUpBtns[i];
        const popUpTargetId = closeBtn.getAttribute("data-close-pop-up");
        const popUpStyle = getPopUpById(popUpTargetId).style;
        closeBtn.addEventListener("click", () => {
            popUpStyle.transform = "translate(-50%, -50%) scale(.9)";
            popUpStyle.opacity = 0;
            popUpStyle.visibility = "hidden";
        });
    }
}

function effectOffsetBottom(obj) {
    if (window.innerWidth > 576) {
        const box = document.querySelector(obj.item);
        const offItem = document.querySelector(obj.offset);
        const boxOff = normalize(offsetBottom(offItem), window.innerHeight);
        const rect = box.getBoundingClientRect();
        if (boxOff < 0 && rect.bottom > 0 && rect.top < window.innerHeight) {
            const transform = boxOff * obj.factor;
            box.style.transition = "transform .5s ease-out, opacity 2s ease-out";
            box.style.transform = `translate(${obj.x}%, ${transform}px)`;
        }
    }
}

function clamp(value, min, max) {
    if (value <= min) return min;
    else if (value >= max) return max;
    return value;
}

function offsetBottom(item) {
    const element = item;
    const rect = element.getBoundingClientRect();
    let offsetBottom = rect.top - window.innerHeight;
    return offsetBottom;
}

function removeFromString(str, charArray) {
    charArray.forEach(c => {
        str = str.replace(c, "");
    });
    return str;
}

function l(x) {
    console.log(x);
}

function generateServices(element, width, height, count, a, b) {
    const grid = getGrid(factor(count));
    const rows = grid[1];
    const columns = grid[0];
    const container = document.querySelector(element);
    for (let row = 0; row < rows; row++) {
        for (let column = 0; column < columns; column++) {
            const image = `http://picsum.photos/id/${Math.round(Math.random() * 70)}/800/500`;
            const xBox = column * width;
            const yBox = row * height + (column * height);
            const i = row * columns + column;
            const index = `box-index-${i}`;
            const boxHtml = `
            <div class="service-box row center-elements position-absolute ${index}" 
            style='width: ${width}%;height: ${height}%;left: ${xBox}%; top: ${yBox}%;'>
                <img class="img-img max-size" src='${image}' alt="${a[i]}" />
                    <div class="white pointer-area max-size z-1 position-absolute"></div>
                    <div class="white position-absolute p-2">
                        <h1 class="text-align-center f-6 font-jos">${a[i]}</h1>
                        <p class="font-com">${b[i]}</p>
                    </div>    
                
            </div>
                `;
            container.insertAdjacentHTML("afterbegin", boxHtml);
        }
    }
    for (let i = 0; i < container.children.length; i++) {
        const currentBox = container.children[i];
        const pointerAreaBox = currentBox.querySelector(".pointer-area");
        currentBox.style.transition = "left 1s ease-out, top 1s ease-out, transform .2s cubic-bezier(.03,.88,.78,.95)";
        pointerAreaBox.addEventListener("mouseenter", () => {
            currentBox.style.zIndex = 1;
            currentBox.style.transform = "scale(1.3)";
        });
        pointerAreaBox.addEventListener("mouseout", () => {
            currentBox.style.zIndex = 0;
            currentBox.style.transform = "scale(1)";
        });
    }
    container.addEventListener("mousemove", () => {
        const rect = getMousePosInElementCenter(container);
        const x = rect.x, y = rect.y;
        const containerWidth = container.getBoundingClientRect().width, containerHeight = container.getBoundingClientRect().height;
        const normX = normalize(x, containerWidth);
        const normY = normalize(y, containerHeight);
        if (normX < -.18 || normX > .18 || normY < -.22 || normY > .22) {
            for (let i = 0; i < container.children.length; i++) {
                const currentBox = container.children[i];
                const curX = currentBox.style.left.replace("%", "");
                const curY = currentBox.style.top.replace("%", "");
                currentBox.style.left = `${curX - normX * 2}%`;
                currentBox.style.top = `${curY - normY * 3}%`;
            }
        }
    });
    centerServiceElement(a.length / 2);
    const findBtn = document.querySelector(".center-service-btn");
    findBtn.onclick = () => centerServiceElement(a.length / 2);
    function centerServiceElement(index) {
        const container = document.querySelector(".services-content");
        const dBox = container.children[index];
        const dx = dBox.style.left.replace("%", "");
        const dy = dBox.style.top.replace("%", "");
        for (let i = 0; i < container.children.length; i++) {
            const currentBox = container.children[i];
            const curX = Number(currentBox.style.left.replace("%", ""));
            const curY = Number(currentBox.style.top.replace("%", ""));
            currentBox.style.left = `${curX - dx + 50 - width / 2}%`;
            currentBox.style.top = `${curY - dy + 50 - height / 2}%`;
        }
    }
}

function getGrid(array) {
    if (array.length < 2) {
        array.push(1);
        return array;
    }
    if (array.length == 2) return array;
    while (array.length > 2) {
        let f = array[0] * array[1];
        array.shift();
        array.shift();
        array.unshift(f);
    }
    return array;
}

function factor(nr) {
    if (nr === 1) return [1];
    if (nr === 2) return [2];
    let factors = [];
    while (nr > 1) {
        for (let i = 2; i <= nr; i++) {
            if (nr % i) continue;
            factors.push(i);
            nr = nr / i;
            break;
        }
    }
    return factors;
}

function activeEmployeeSelection() {
    const employee = document.querySelector(".employee-container");
    const btn = employee.querySelector(".select-employee-btn");
    const input = document.querySelector(".input-employee-selection");
    const name = employee.querySelector(".employee-name-selection");
    btn.onclick = () => {
        input.value = name.getAttribute("data-employee-name");
    }
}

//activeDateInput();
function activeDateInput() {
    const currentYear = new Date().getFullYear();
    const daysOfWeek = ["Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb"];
    const months = ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'];
    const ui = document.querySelector(".date-ui");
    const weekEl = document.querySelector(".days-of-week");
    const input = document.querySelector(".input-date");
    const selector = ui.querySelector("select");

    months.forEach(m => {
        const element = `<option>${m}</option>`;
        selector.insertAdjacentHTML("beforeend", element);
    });
    for (let i = 0; i < daysOfWeek.length; i++) {
        const day = daysOfWeek[i];
        const element = `<div class="f-0">${day}</div>`;
        weekEl.insertAdjacentHTML("beforeend", element);
    }
    selector.addEventListener("change", () => slideTo(selector.selectedIndex));

    const contentContainer = document.querySelector(".date-content-slider");
    let lastSelectedItem;
    let fromThisMonth;
    for (let i = 0; i < 12; i++) {
        const item = document.createElement("div");
        item.className = "date-container-item";
        contentContainer.append(item);
        generateMonth(i, item);
        for (let j = 0; j < item.children.length; j++) {
            const dateItem = item.children[j];
            const monthContainer = dateItem.getAttribute("data-month-container");
            const month = dateItem.getAttribute("data-month");
            if (!(monthContainer == "0" && month == "12") && !(monthContainer == "11" && month == "01")) {
                dateItem.classList.add("cursor-pointer");
                dateItem.onclick = () => {
                    try {
                        fromThisMonth = lastSelectedItem.getAttribute("data-from-this-month");
                        lastSelectedItem.style.backgroundColor = fromThisMonth == "true" ? "black" : "white";
                        lastSelectedItem.style.color = fromThisMonth == "true" ? "white" : "black";
                    } catch{ }
                    input.value = dateItem.getAttribute("data-date");
                    dateItem.style.backgroundColor = "rgb(0, 255, 0)";
                    dateItem.style.color = "black";
                    lastSelectedItem = dateItem;
                }
            }
        }
    }

    let containerIndex = 0;
    const next = document.querySelector(".next-container-date");
    const prev = document.querySelector(".prev-container-date");
    contentContainer.style.transition = "transform .4s var(--main-bezier)";
    const todayMonth = new Date().getMonth();
    selectMonth(todayMonth);
    slideTo(todayMonth);
    next.onclick = () => {
        containerIndex = containerIndex >= 11 ? 11 : containerIndex += 1;
        selectMonth(containerIndex);
        slideTo(containerIndex);
    }
    prev.onclick = () => {
        containerIndex = containerIndex <= 0 ? 0 : containerIndex -= 1;
        selectMonth(containerIndex);
        slideTo(containerIndex);
    }

    function slideTo(index) {
        contentContainer.style.transform = `translateX(-${100 * index}%)`;
        containerIndex = index;
    }

    function selectMonth(index) {
        selector.selectedIndex = index;
    }

    function generateMonth(monthIndex, container) {
        const cont = typeof (container) === "string" ? document.querySelector(container) : container;
        const monthToGenerate = monthIndex;
        let currentDay, currentMonth;
        const daysOfCurrentMonth = daysOfMonth(monthToGenerate);
        const startingDayOfCurrentMonth = startingDay(monthToGenerate);
        let startGray = daysOfMonth(monthToGenerate - 1) - startingDayOfCurrentMonth + 1,
            tempDay = 0, bgColor, color;

        currentMonth = monthIndex + 1 <= 1 ? 12 : monthIndex;
        for (let i = 0; i < 6 * 7; i++) {
            if (i >= startingDayOfCurrentMonth & i <= daysOfCurrentMonth + startingDayOfCurrentMonth - 1) {
                attrBool = true;
                color = "white";
                bgColor = "black";
                tempDay++;
                currentDay = tempDay;
                currentMonth = monthIndex + 1;
            }
            else {
                if (currentDay === daysOfCurrentMonth && i > startingDayOfCurrentMonth) {
                    startGray = 1;
                    currentMonth++;
                    currentMonth = currentMonth > 12 ? 1 : currentMonth;
                }
                attrBool = false;
                color = "black";
                bgColor = "white";
                currentDay = startGray;
                startGray++;
            }

            currentMonth = (currentMonth < 10 && currentMonth.toString().length < 2) ? ("0" + currentMonth) : currentMonth;
            currentDay = currentDay < 10 ? ("0" + currentDay) : currentDay;
            const element = `<div data-from-this-month=${attrBool} data-month-container=${monthIndex} data-month=${currentMonth} data-date=${currentDay}/${currentMonth}/${currentYear}
                    class="f-0 item-date row center-elements" 
                    style="background-color: ${bgColor};color: ${color}">${currentDay}</div>`;
            cont.insertAdjacentHTML("beforeend", element);
        }

        function daysOfMonth(month) { return new Date(2020, month + 1, 0).getDate(); }
        function startingDay(month) { return new Date(2020, month, 1).getDay(); }
    }
}
//maskString("###.###.###-##", "83712398401");
function maskString(mask, str) {
    const numberChar = "#";
    const maskArray = mask.split("");
    const strArray = str.split("");
    let factor = 0;
    for (let i = 0; i < maskArray.length; i++) {
        const char = maskArray[i];
        if (char === numberChar) {
            maskArray.splice(i, 1, strArray[i - factor])
        } else {
            factor++;
            continue;
        }
    }
    return maskArray.join("");
}

async function isSigned() {
    const isSigned = false;
    await sendRequest("post", "/Account/IsSigned", {}, { OnSuccess: res => isSigned = res.isSigned });
    return isSigned;
}

async function generateNewSignature() {
    let token = null;
    await sendRequest("post", "/SJWT/GenerateSignature", { }, { OnSuccess: res => token = res.token, Loader: ".main-loader" });
    return token;
}