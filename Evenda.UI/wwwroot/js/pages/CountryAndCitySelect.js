$('document').ready(() => {
    VirtualSelect.init({
        ele: '#country-select',
        dropboxWrapper: 'self',
        dropboxWidth: "100%",
        focusSelectedOptionOnOpen: true,
        showDropboxAsPopup: true,
        popupDropboxBreakpoint: '576px',
        popupPosition: 'center',
        maxWidth: '100%',
        search: true,
        searchByStartsWith: true,
        hideClearButton: true,
        labelRenderer: countryFlagRenderer,
        selectedLabelRenderer: countryFlagRenderer
    });

    function countryFlagRenderer(data) {
        let countryCode = data.label.replace(/\s+/g, '-').toLowerCase();
        let prefix = `<i class="flag flag-${countryCode}"></i>`;
        return `${prefix}${data.label}`;
    }

    $.ajax({
        url: "https://api.countrystatecity.in/v1/countries",
        method: "GET",
        headers: {
            "X-CSCAPI-KEY": "amtFc3ptQlo5OUhsV2ZPSFBKM3dFRms2N1RQR3kzMFpETTByRmIzOQ=="
        },
        success: function (response) {
            if (!response.error) {
                var countryOpts = response.map(function (country) {
                    return { label: country.name, value: country.name, customData: country.iso2 };
                });

                document.querySelector('#country-select').setOptions(countryOpts);

            } else {
                console.log('Error:', response.msg);
            }
        },
        error: function (xhr, status, error) {
            console.log('An error occurred:', error);
        }
    });


    VirtualSelect.init({
        ele: '#city-select',
        dropboxWrapper: 'self',
        dropboxWidth: "100%",
        focusSelectedOptionOnOpen: true,
        showDropboxAsPopup: true,
        popupDropboxBreakpoint: '576px',
        popupPosition: 'center',
        maxWidth: '100%',
        search: true,
        searchByStartsWith: true,
        hideClearButton: true,
    });


    $('#country-select').change(() => {
        var country = document.querySelector('#country-select').getSelectedOptions();
        var iso2 = country ? country.customData : undefined;
        var city_select = document.querySelector('#city-select');

        city_select.reset();

        if (!iso2) {
            city_select.disable();
            return;
        }

        city_select.enable();

        $.ajax({
            url: `https://api.countrystatecity.in/v1/countries/${iso2}/cities`,
            type: 'GET',
            headers: {
                "X-CSCAPI-KEY": "amtFc3ptQlo5OUhsV2ZPSFBKM3dFRms2N1RQR3kzMFpETTByRmIzOQ=="
            },
            success: function (response) {
                if (!response.error) {
                    var cityOpts = response.map(function (city) {
                        var name = city.name.normalize('NFD').replace(/[\u0300-\u036f]/g, '');
                        return { label: name, value: name };
                    });
                    city_select.setOptions(cityOpts);
                } else {
                    console.log('Error:', response.msg);
                }
            },
            error: function (xhr, status, error) {
                console.log('An error occurred:', error);
            }
        });

    });
});