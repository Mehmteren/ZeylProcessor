const fileInput = document.getElementById('file');
const uploadForm = document.getElementById('uploadForm');
const processBtn = document.getElementById('processBtn');
const loading = document.getElementById('loading');
const result = document.getElementById('result');
const fileInfo = document.getElementById('fileInfo');
const uploadArea = document.querySelector('.upload-area');

let downloadId = null;

document.addEventListener('DOMContentLoaded', function () {
    fileInput.addEventListener('change', handleFileSelect);

    setupDragAndDrop();

    uploadForm.addEventListener('submit', handleFormSubmit);

    document.getElementById('downloadBtn').addEventListener('click', handleDownload);
});

function handleFileSelect() {
    if (fileInput.files[0]) {
        document.getElementById('fileName').textContent = fileInput.files[0].name;
        fileInfo.style.display = 'block';
        processBtn.disabled = false;
    }
}

function setupDragAndDrop() {
    uploadArea.addEventListener('dragover', e => {
        e.preventDefault();
        uploadArea.classList.add('dragover');
    });

    uploadArea.addEventListener('dragleave', () => {
        uploadArea.classList.remove('dragover');
    });

    uploadArea.addEventListener('drop', e => {
        e.preventDefault();
        uploadArea.classList.remove('dragover');
        if (e.dataTransfer.files[0]) {
            fileInput.files = e.dataTransfer.files;
            fileInput.dispatchEvent(new Event('change'));
        }
    });
}

async function handleFormSubmit(e) {
    e.preventDefault();

    if (!fileInput.files[0]) return;

    const formData = new FormData();
    formData.append('file', fileInput.files[0]);

    updateUIState('loading');

    try {
        const response = await fetch('/api/zeyil/process', {
            method: 'POST',
            body: formData
        });

        const data = await response.json();

        if (data.success) {
            downloadId = data.downloadId;
            document.getElementById('resultText').textContent = `${data.processedCount} kayıt işlendi.`;
            updateUIState('success');
            showAlert('İşlem başarıyla tamamlandı!', 'success');
        } else {
            showAlert(data.message, 'danger');
            updateUIState('error');
        }
    } catch (error) {
        console.error('Error:', error);
        showAlert('Bağlantı hatası oluştu. Lütfen tekrar deneyin.', 'danger');
        updateUIState('error');
    }
}

function handleDownload() {
    if (downloadId) {
        window.location.href = `/api/zeyil/download/${downloadId}`;
    }
}

function updateUIState(state) {
    switch (state) {
        case 'loading':
            processBtn.disabled = true;
            loading.style.display = 'block';
            result.style.display = 'none';
            clearAlert();
            break;

        case 'success':
            loading.style.display = 'none';
            result.style.display = 'block';
            processBtn.disabled = false;
            break;

        case 'error':
            loading.style.display = 'none';
            processBtn.disabled = false;
            break;
    }
}

function showAlert(message, type) {
    document.getElementById('alert').innerHTML = `
        <div class="alert alert-${type} alert-dismissible fade show" role="alert">
            ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        </div>
    `;
}

function clearAlert() {
    document.getElementById('alert').innerHTML = '';
}