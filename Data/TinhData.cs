using System;
using System.Collections.Generic;

namespace ThanhBuoi.Data
{
    public class TinhData
    {
        private static TinhData instance = null;
        private static readonly object padlock = new object();

        private List<string> tinhThanh;

        private TinhData()
        {
            tinhThanh = new List<string>()
                {
                    "Tỉnh An Giang", "Tỉnh Bà Rịa - Vũng Tàu", "Tỉnh Bạc Liêu", "Tỉnh Bắc Kạn", "Tỉnh Bắc Giang", "Tỉnh Bắc Ninh",
                    "Tỉnh Bến Tre", "Tỉnh Bình Dương", "Tỉnh Bình Định", "Tỉnh Bình Phước", "Tỉnh Bình Thuận", "Tỉnh Cà Mau",
                    "Tỉnh Cao Bằng", "Thành phố Cần Thơ", "Thành phố Đà Nẵng", "Tỉnh Đắk Lắk", "Tỉnh Đắk Nông", "Tỉnh Điện Biên", "Tỉnh Đồng Nai",
                    "Tỉnh Đồng Tháp", "Tỉnh Gia Lai", "Tỉnh Hà Giang", "Tỉnh Hà Nam", "Thành phố Hà Nội", "Tỉnh Hà Tĩnh", "Thành phố Hải Dương",
                    "Tỉnh Hải Phòng", "Tỉnh Hậu Giang", "Tỉnh Hòa Bình", "Tỉnh Hưng Yên", "Tỉnh Khánh Hòa", "Tỉnh Kiên Giang",
                    "Tỉnh Kon Tum", "Tỉnh Lai Châu", "Tỉnh Lạng Sơn", "Tỉnh Lào Cai", "Tỉnh Lâm Đồng", "Tỉnh Long An", "Tỉnh Nam Định",
                    "Tỉnh Nghệ An", "Tỉnh Ninh Bình", "Tỉnh Ninh Thuận", "Tỉnh Phú Thọ", "Tỉnh Phú Yên", "Tỉnh Quảng Bình",
                    "Tỉnh Quảng Nam", "Tỉnh Quảng Ngãi", "Tỉnh Quảng Ninh", "Tỉnh Quảng Trị", "Tỉnh Sóc Trăng", "Tỉnh Sơn La",
                    "Tỉnh Tây Ninh", "Tỉnh Thái Bình", "Tỉnh Thái Nguyên", "Tỉnh Thanh Hóa", "Tỉnh Thừa Thiên - Huế", "Tỉnh Tiền Giang",
                    "Thành phố Hồ Chí Minh", "Tỉnh Trà Vinh", "Tỉnh Tuyên Quang", "Tỉnh Vĩnh Long", "Tỉnh Vĩnh Phúc", "Tỉnh Yên Bái"
                };

        }

        public static TinhData GetInstance()
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new TinhData();
                }
                return instance;
            }
        }

        public List<string> GetTinhThanh()
        {
            return tinhThanh;
        }
    }
}
