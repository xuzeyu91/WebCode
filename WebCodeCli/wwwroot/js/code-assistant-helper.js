// 编程助手页面辅助函数

// 滚动输出容器到底部
window.scrollOutputToBottom = function() {
    requestAnimationFrame(() => {
        const container = document.getElementById('output-container');
        if (container) {
            container.scrollTop = container.scrollHeight;
        }
    });
};

// 滚动聊天消息容器到底部
window.scrollChatToBottom = function() {
    requestAnimationFrame(() => {
        const container = document.getElementById('chat-messages-container');
        if (container) {
            container.scrollTop = container.scrollHeight;
        }
    });
};

// 滚动到指定容器底部（通用方法）
window.scrollToBottom = function(containerId) {
    requestAnimationFrame(() => {
        const container = document.getElementById(containerId);
        if (container) {
            container.scrollTop = container.scrollHeight;
        }
    });
};

// 获取滚动容器信息（用于向上加载更多时保持滚动位置）
window.getScrollInfo = function(containerId) {
    const container = document.getElementById(containerId);
    if (!container) {
        return { scrollTop: 0, scrollHeight: 0, clientHeight: 0 };
    }
    return {
        scrollTop: container.scrollTop,
        scrollHeight: container.scrollHeight,
        clientHeight: container.clientHeight
    };
};

// 在内容“顶部追加”后恢复滚动位置，避免视图跳动
window.restoreScrollAfterPrepend = function(containerId, prevScrollHeight, prevScrollTop) {
    requestAnimationFrame(() => {
        const container = document.getElementById(containerId);
        if (!container) {
            return;
        }

        const newScrollHeight = container.scrollHeight;
        const delta = newScrollHeight - prevScrollHeight;
        container.scrollTop = prevScrollTop + delta;
    });
};

// 调整iframe高度以适应内容
window.adjustIframeHeight = function() {
    const iframe = document.getElementById('html-preview-frame');
    if (iframe && iframe.contentWindow) {
        try {
            const contentHeight = iframe.contentWindow.document.body.scrollHeight;
            if (contentHeight > 0) {
                iframe.style.height = contentHeight + 'px';
            }
        } catch (e) {
            // 跨域或其他错误，使用默认高度
            console.log('无法访问iframe内容:', e.message);
        }
    }
};

// 监听iframe加载完成事件
window.setupIframeAutoResize = function() {
    const iframe = document.getElementById('html-preview-frame');
    if (iframe) {
        iframe.onload = function() {
            window.adjustIframeHeight();
        };
        
        // 使用MutationObserver监听srcdoc变化
        const observer = new MutationObserver(function() {
            setTimeout(() => window.adjustIframeHeight(), 100);
        });
        
        observer.observe(iframe, {
            attributes: true,
            attributeFilter: ['srcdoc']
        });
    }
};

// 下载文件
window.downloadFile = function(fileName, content, mimeType = 'application/octet-stream') {
    const blob = new Blob([content], { type: mimeType });
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = fileName;
    document.body.appendChild(a);
    a.click();
    window.URL.revokeObjectURL(url);
    document.body.removeChild(a);
};

// 下载Base64文件
window.downloadBase64File = function(fileName, base64Content, mimeType = 'application/octet-stream') {
    const byteCharacters = atob(base64Content);
    const byteNumbers = new Array(byteCharacters.length);
    for (let i = 0; i < byteCharacters.length; i++) {
        byteNumbers[i] = byteCharacters.charCodeAt(i);
    }
    const byteArray = new Uint8Array(byteNumbers);
    const blob = new Blob([byteArray], { type: mimeType });
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = fileName;
    document.body.appendChild(a);
    a.click();
    window.URL.revokeObjectURL(url);
    document.body.removeChild(a);
};

// 应用代码高亮（使用Prism.js）
window.applyCodeHighlight = function() {
    console.log('准备应用代码高亮，Prism 状态:', typeof Prism !== 'undefined' ? '已加载' : '未加载');
    
    // 检查 Prism 是否已加载
    if (typeof Prism !== 'undefined') {
        requestAnimationFrame(() => {
            try {
                Prism.highlightAll();
                console.log('✓ 代码高亮应用成功');
            } catch (error) {
                console.error('✗ 应用代码高亮失败:', error);
            }
        });
        return;
    }
    
    // 如果未加载，尝试等待加载
    let attempts = 0;
    const maxAttempts = 50; // 最多等待5秒
    
    const tryHighlight = () => {
        attempts++;
        console.log(`等待 Prism 加载... (${attempts}/${maxAttempts})`);
        
        if (typeof Prism !== 'undefined') {
            requestAnimationFrame(() => {
                try {
                    Prism.highlightAll();
                    console.log('✓ 代码高亮应用成功（延迟加载）');
                } catch (error) {
                    console.error('✗ 应用代码高亮失败:', error);
                }
            });
        } else if (attempts < maxAttempts) {
            setTimeout(tryHighlight, 100);
        } else {
            console.error('✗ Prism.js 加载超时。可能的原因：');
            console.error('1. prism.min.js 文件不存在或损坏');
            console.error('2. 文件路径配置错误');
            console.error('3. 浏览器扩展阻止了脚本加载');
            console.error('请检查控制台是否有 404 错误，并确认文件路径: ./js/prism.min.js');
        }
    };
    
    tryHighlight();
};

// 触发文件上传对话框
window.triggerFileUpload = function(inputId) {
    const input = document.getElementById(inputId);
    if (input) {
        input.click();
    }
};

// 读取文件内容为Base64
window.readFileAsBase64 = async function(inputId) {
    const input = document.getElementById(inputId);
    if (!input || !input.files || input.files.length === 0) {
        return null;
    }

    const file = input.files[0];
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.onload = function(e) {
            const base64 = e.target.result.split(',')[1]; // 移除 data:*/*;base64, 前缀
            resolve({
                fileName: file.name,
                fileSize: file.size,
                base64Content: base64
            });
        };
        reader.onerror = reject;
        reader.readAsDataURL(file);
    });
};

// 安全地设置 Cookie（供共享工作区使用）
window.setShareCookie = function(name, value, expires) {
    if (!name || typeof name !== 'string') {
        console.error('Cookie 名称无效');
        return;
    }
    // 转义特殊字符，防止 Cookie 注入
    const safeName = encodeURIComponent(name);
    const safeValue = encodeURIComponent(value || '');
    document.cookie = `${safeName}=${safeValue}; path=/; expires=${expires}; SameSite=Lax`;
};

// 清除共享 Cookie
window.clearShareCookie = function(name) {
    if (!name || typeof name !== 'string') return;
    const safeName = encodeURIComponent(name);
    document.cookie = `${safeName}=; path=/; expires=Thu, 01 Jan 1970 00:00:00 GMT`;
};

