const loaderTemplate = '<div class="lds-ring"><div></div><div></div><div></div><div></div></div>'

const loaderForElement = (selectQuery) => {
    const elem = $(selectQuery)
    elem.append(loaderTemplate);
}
const removeLoaderFor = (selectQuery) => {
    const elem = $(selectQuery)
    elem.children(".lds-ring").remove();
}