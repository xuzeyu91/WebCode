// 本地化辅助函数
console.log('🔧 localization-helper.js 正在加载...');

/**
 * 本地化管理器
 * 使用 localStorage 存储语言设置（简单可靠，无需依赖 IndexedDB）
 */
window.localizationHelper = (function() {
    const STORAGE_KEY = 'webcli_language';
    const DEFAULT_LANGUAGE = 'zh-CN';
    
    let currentLanguage = DEFAULT_LANGUAGE;
    let translations = {};

    let initPromise = null;
    let isInitialized = false;

    /**
     * 初始化本地化 - 使用 localStorage
     * @returns {Promise<string>} 当前语言
     */
    async function init() {
        // 如果已经初始化完成，直接返回当前语言
        if (isInitialized) {
            return currentLanguage;
        }

        // 如果正在初始化，返回同一个 Promise 避免重复初始化
        if (initPromise) {
            return initPromise;
        }

        initPromise = (async () => {
            try {
                console.log('🔄 开始初始化本地化...');
                
                // 从 localStorage 读取保存的语言设置
                try {
                    const savedLanguage = localStorage.getItem(STORAGE_KEY);
                    if (savedLanguage) {
                        currentLanguage = savedLanguage;
                        console.log(`✅ 从 localStorage 加载语言设置: ${currentLanguage}`);
                        isInitialized = true;
                        return currentLanguage;
                    }
                } catch (storageError) {
                    console.warn('⚠️ 从 localStorage 读取语言设置失败:', storageError);
                }
                
                // 如果 localStorage 没有设置，尝试从浏览器语言自动检测
                const browserLang = navigator.language || navigator.userLanguage;
                if (browserLang) {
                    // 标准化语言代码
                    if (browserLang.startsWith('zh')) {
                        currentLanguage = 'zh-CN';
                    } else if (browserLang.startsWith('en')) {
                        currentLanguage = 'en-US';
                    } else if (browserLang.startsWith('ja')) {
                        currentLanguage = 'ja-JP';
                    } else if (browserLang.startsWith('ko')) {
                        currentLanguage = 'ko-KR';
                    }
                }
                
                isInitialized = true;
                console.log(`✅ 本地化初始化完成，当前语言: ${currentLanguage}`);
                return currentLanguage;
            } catch (error) {
                console.error('❌ 初始化本地化失败:', error);
                isInitialized = true; // 即使失败也标记为已初始化，避免重复尝试
                return currentLanguage;
            }
        })();

        return initPromise;
    }

    /**
     * 获取当前语言
     * @returns {string}
     */
    function getCurrentLanguage() {
        return currentLanguage;
    }

    /**
     * 设置当前语言 - 使用 localStorage
     * @param {string} language - 语言代码
     * @returns {Promise<boolean>}
     */
    async function setCurrentLanguage(language) {
        try {
            if (!language) {
                console.error('❌ 设置语言失败: 语言代码为空');
                return false;
            }

            currentLanguage = language;
            
            // 保存到 localStorage
            try {
                localStorage.setItem(STORAGE_KEY, language);
            } catch (storageError) {
                console.warn('⚠️ 保存语言设置到 localStorage 失败:', storageError);
            }
            
            console.log(`✅ 语言已设置为: ${language}`);
            return true;
        } catch (error) {
            console.error('❌ 设置语言失败:', error);
            return false;
        }
    }

    /**
     * 加载翻译资源
     * @param {string} language - 语言代码
     * @param {Object} translationData - 翻译数据
     */
    function loadTranslations(language, translationData) {
        try {
            if (!language || !translationData) {
                console.error('❌ 加载翻译资源失败: 参数无效');
                return false;
            }

            translations[language] = translationData;
            console.log(`✅ 已加载 ${language} 的翻译资源`);
            return true;
        } catch (error) {
            console.error('❌ 加载翻译资源失败:', error);
            return false;
        }
    }

    /**
     * 获取翻译文本
     * @param {string} key - 翻译键
     * @param {Object} params - 参数对象（用于插值）
     * @returns {string}
     */
    function translate(key, params = {}) {
        try {
            if (!key) {
                return '';
            }

            // 获取当前语言的翻译
            const langTranslations = translations[currentLanguage] || {};
            
            // 支持嵌套键（如 "common.save"）
            const keys = key.split('.');
            let value = langTranslations;
            
            for (const k of keys) {
                if (value && typeof value === 'object' && k in value) {
                    value = value[k];
                } else {
                    // 如果找不到翻译，返回键本身
                    console.warn(`⚠️ 翻译键未找到: ${key} (语言: ${currentLanguage})`);
                    return key;
                }
            }

            // 如果找到的不是字符串，返回键
            if (typeof value !== 'string') {
                return key;
            }

            // 参数插值
            let result = value;
            Object.keys(params).forEach(paramKey => {
                const placeholder = `{${paramKey}}`;
                result = result.replace(new RegExp(placeholder, 'g'), params[paramKey]);
            });

            return result;
        } catch (error) {
            console.error('❌ 翻译失败:', error);
            return key;
        }
    }

    /**
     * 获取支持的语言列表
     * @returns {Array}
     */
    function getSupportedLanguages() {
        return [
            { code: 'zh-CN', name: '简体中文', nativeName: '简体中文' },
            { code: 'en-US', name: 'English', nativeName: 'English' },
            { code: 'ja-JP', name: 'Japanese', nativeName: '日本語' },
            { code: 'ko-KR', name: 'Korean', nativeName: '한국어' }
        ];
    }

    /**
     * 格式化日期
     * @param {Date|string} date - 日期
     * @param {string} format - 格式（'short', 'long', 'time'）
     * @returns {string}
     */
    function formatDate(date, format = 'short') {
        try {
            const dateObj = typeof date === 'string' ? new Date(date) : date;
            
            if (!(dateObj instanceof Date) || isNaN(dateObj)) {
                return '';
            }

            const options = {
                'short': { year: 'numeric', month: '2-digit', day: '2-digit' },
                'long': { year: 'numeric', month: 'long', day: 'numeric' },
                'time': { hour: '2-digit', minute: '2-digit', second: '2-digit' },
                'datetime': { year: 'numeric', month: '2-digit', day: '2-digit', hour: '2-digit', minute: '2-digit' }
            };

            return new Intl.DateTimeFormat(currentLanguage, options[format] || options.short).format(dateObj);
        } catch (error) {
            console.error('❌ 日期格式化失败:', error);
            return '';
        }
    }

    /**
     * 格式化数字
     * @param {number} number - 数字
     * @param {number} decimals - 小数位数
     * @returns {string}
     */
    function formatNumber(number, decimals = 0) {
        try {
            if (typeof number !== 'number' || isNaN(number)) {
                return '';
            }

            return new Intl.NumberFormat(currentLanguage, {
                minimumFractionDigits: decimals,
                maximumFractionDigits: decimals
            }).format(number);
        } catch (error) {
            console.error('❌ 数字格式化失败:', error);
            return String(number);
        }
    }

    /**
     * 从服务器获取翻译文件
     * @param {string} filePath - 翻译文件路径
     * @returns {Promise<string>} 翻译文件内容（JSON字符串）
     */
    async function fetchTranslationFile(filePath) {
        try {
            console.log(`📥 正在加载翻译文件: ${filePath}`);
            const response = await fetch(filePath, { cache: 'no-store' });
            if (!response.ok) {
                console.error(`❌ 加载翻译文件失败: HTTP ${response.status}`);
                return '';
            }
            const json = await response.text();
            console.log(`✅ 翻译文件加载成功: ${filePath}`);
            return json;
        } catch (error) {
            console.error(`❌ 加载翻译文件失败: ${error.message}`);
            return '';
        }
    }

    // 初始化
    init();

    // 导出公共 API
    return {
        init,
        getCurrentLanguage,
        setCurrentLanguage,
        loadTranslations,
        translate,
        t: translate, // 简写别名
        getSupportedLanguages,
        formatDate,
        formatNumber,
        fetchTranslationFile,
        isReady: () => isInitialized
    };
})();

console.log('✅ localization-helper.js 加载完成');

