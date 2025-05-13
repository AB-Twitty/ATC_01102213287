$(document).ready(function () {
    $("#event-date").datepicker({
        startView: 1,
        todayBtn: "linked",
        keyboardNavigation: false,
        forceParse: false,
        autoclose: true,
        format: "mm/dd/yyyy",
        startDate: new Date()
    });

    VirtualSelect.init({
        ele: '.multiple-select',
        dropboxWrapper: 'self',
        optionsCount: 8,
        allowNewOption: true,
        optionsSelectedText: 'options selected',
        allOptionsSelectedText: 'All',
        showValueAsTags: true,
        noOfDisplayValues: 5,
        alwaysShowSelectedOptionsLabel: false,
        dropboxWidth: "100%",
        focusSelectedOptionOnOpen: true,
        showDropboxAsPopup: true,
        popupDropboxBreakpoint: '576px',
        popupPosition: 'center',
        hideValueTooltipOnSelectAll: true,
        maxWidth: '100%'
    });

    $(".summernote").summernote({
        callbacks: {
            onChange: function (contents, $editable) {
                $('#description-input').val(contents); 
            }
        }
    });

    $(".clockpicker").clockpicker({
        twelvehour: true, 
        autoclose: true
    });


    var fileArray = [];

    $('#upload-btn').on('click', function () {
        $('#image-input').click();
    });

    $('#image-input').on('change', function () {
        var files = this.files;
        var imageContainer = $('#images-gallery');

        for (var i = 0; i < files.length; i++) {
            var file = files[i];

            if (!file.type.startsWith('image/')) {
                alert('Only image files are allowed.');
                continue;
            }
            if (file.size > 5 * 1024 * 1024) {
                alert('File size must be less than 5MB.');
                continue;
            }

            fileArray.push(file);

            (function (index) {
                var reader = new FileReader();
                var isFirstImage = fileArray.length === 1;

                reader.onload = function (e) {
                    var imgHtml = `
                        <div class="col-xl-3 col-lg-4 col-md-6 col-sm-12 d-flex flex-column image text-center p-2">
                            <div class="shadow d-flex flex-column flex-grow-1 border-2 border">
                                <img src="${e.target.result}" class="img-fluid pb-1" />
                                <button class="img-del" data-idx="${index}">
                                    <i class="fa fa-close"></i>
                                </button>
                                <div class="d-flex mt-auto">
                                    <div class="w-100 mb-1">
                                        <input ${isFirstImage ? 'checked' : ''} class="form-check-input" type="radio" name="thumbnailIdx" data-idx="${index}" value="${index}" />
                                        <label class="m-0">Thumbnail</label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    `;
                    imageContainer.append(imgHtml);
                };

                reader.readAsDataURL(file);
            })(fileArray.length - 1);
        }
    });

    $(document).on('click', '.img-del', function () {
        var isThumbnail = $(this).closest('.image').find('input[name="thumbnailIdx"]').is(':checked');
        var index = $(this).attr('data-idx');
        fileArray.splice(index, 1);
        $(this).closest('.image').remove();

        var dataTransfer = new DataTransfer();
        fileArray.forEach(function (file) {
            dataTransfer.items.add(file);
        });
        $('#image-input')[0].files = dataTransfer.files;

        $('#images-gallery .image').each(function (i) {
            $(this).find('.img-del').attr('data-idx', i);
            $(this).find('input[name="thumbnailIdx"]').attr('data-idx', i).val(i);
        });

        if (isThumbnail && fileArray.length > 0) {
            $('#images-gallery .image').first().find('input[name="thumbnailIdx"]').prop('checked', true);
        }
    });
});