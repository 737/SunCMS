sun.form = function () {
    
    
    function setForm () {
            var $mainFrame = $('#mainFrame');
        
            $mainFrame.on('submit', 'form:first', function () {
                $(this).ajaxForm();
                return false;
            });

            $mainFrame.on('click', 'form input[name="reset"]', function () {
                $message = $('#action .message > div').hide()
            })
        }

    return {
        // load全局的form,,将from转变使用 from.js提交
        loadForm: function ($domForm) {
            // old
            //$('body').ajaxForm();
            //return false;
            
            // new
            setForm();
        },
        submitForm: function ($domForm, options, callBack) {
            var defaultOption = {
                crud: 'r',
                pac: 'p',
                async: true,
                type: "post",
                url: '',
                dataType: null,
                beforeSubmit: function (arr, $form, options) {
                    console.log('submit fire sucess')
                },
                success: function (responseText, statusText) {
                    if (typeof callBack === 'function') {
                        callBack(responseText, statusText);
                    }
                },
                error: function () {
                    console.error('请求页面错误')
                }
            }
            
            _.defaults(options, defaultOption);
            
            if (options.pac === 'a') {
                options.url = '/sun/api/pagelet' + options.url + '.ashx';
            } else {
                options.url = '/sun/pagelet'  + options.url + '.aspx';
            }
            
            options.data = options.data || { };
            options.data.crud = options.crud;
            
            $domForm.ajaxSubmit(options);
            return false;
        },
        add: function function_name ($domForm, options, callBack) {
            this.updateForm($domForm, options, callBack);
        },
        updateForm: function ($domForm, options, callBack) {
            var $btns = $domForm.find('#action .button'),
                $loading = $domForm.find('#action .loading'),
                $message = $domForm.find('#action .message > div'),
                speed = 'normal',
                _me = this;
        
            var defaultOption = {
                crud: 'r',
                pac: 'p',
                async: true,
                type: "post",
                url: '',
                dataType: null,
                beforeSubmit: function (arr, $form, options) {
                    $btns.hide();
                    $loading.fadeIn(speed);
                    $message.hide();
                },
                success: function (responseText, statusText) {
                    $loading.hide();
                    $btns.fadeIn(speed);
                    $message.fadeIn(speed);
                    $message.on('click', '.close', function () {
                        $message.fadeOut(speed)
                    })
                
                    if (typeof callBack === 'function') {
                        callBack(responseText, statusText);
                    }
                },
                error: function () {
                    console.error('请求页面错误')
                }
            }
            
            _.defaults(options, defaultOption);
            
            if (options.pac === 'a') {
                options.url = '/sun/api/pagelet' + options.url + '.ashx';
            } else {
                options.url = '/sun/pagelet'  + options.url + '.aspx';
            }
            
            options.data = options.data || { };
            options.data.crud = options.crud;
            
            $domForm.ajaxSubmit(options);
            return false;
        },
        removeData: function ($domForm, options, callBack) {
            if (options.pac === 'a') {
                options.url = '/sun/api/pagelet' + options.url + '.ashx';
            } else {
                options.url = '/sun/pagelet'  + options.url + '.aspx';
            }
            
            $.get(options.url, options.data);
        }
    }
}();