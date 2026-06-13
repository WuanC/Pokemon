using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Pokemon.Scripts.MyUtils
{
    public class ScreenshotHandler : Singleton<ScreenshotHandler>
    {
        void Update()
        {
            // Khi người chơi nhấn phím 'K', hệ thống sẽ chụp ảnh
            if (Input.GetKeyDown(KeyCode.K))
            {
                // 1. Định nghĩa đường dẫn đến thư mục Screenshots bên trong Assets
                string folderPath = Path.Combine(Application.dataPath, "Screenshots");

                // 2. Kiểm tra nếu thư mục chưa tồn tại thì tự động tạo mới
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // 3. Đặt tên tệp ảnh với thời gian hiện tại
                string fileName = "Screenshot_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";

                // 4. Kết hợp đường dẫn thư mục và tên file
                string filePath = Path.Combine(folderPath, fileName);

                // 5. Tiến hành chụp ảnh và lưu
                ScreenCapture.CaptureScreenshot(filePath);

                // Refresh lại Project Window trong Editor để thấy file ngay lập tức
#if UNITY_EDITOR
                UnityEditor.AssetDatabase.Refresh();
#endif

                Debug.Log("Đã lưu ảnh tại: " + filePath);
            }
        }
        [Button("Capture Screenshot")]
        public void CaptureScreenshot()
        {
            // 1. Định nghĩa đường dẫn đến thư mục Screenshots bên trong Assets
            string folderPath = Path.Combine(Application.dataPath, "Screenshots");

            // 2. Kiểm tra nếu thư mục chưa tồn tại thì tự động tạo mới
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // 3. Đặt tên tệp ảnh với thời gian hiện tại
            string fileName = "Screenshot_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";

            // 4. Kết hợp đường dẫn thư mục và tên file
            string filePath = Path.Combine(folderPath, fileName);

            // 5. Tiến hành chụp ảnh và lưu
            ScreenCapture.CaptureScreenshot(filePath);

            // Refresh lại Project Window trong Editor để thấy file ngay lập tức
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif

            Debug.Log("Đã lưu ảnh tại: " + filePath);
        }
    }
}