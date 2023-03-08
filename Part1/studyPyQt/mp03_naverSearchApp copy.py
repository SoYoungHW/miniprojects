# QT Designer 디자인 사용
import sys
from PyQt5 import uic
from PyQt5.QtWidgets import *
from NaverApi import *

class qtApp(QWidget):
    def __init__(self):
        super().__init__()
        uic.loadUi('./studyPyQt/naverApiSearch.ui',self)

     # 검색 버튼 클릭 시그널 / 슬롯 함수
        self.btnSearch.clicked.connect(self.btnSearchClicked)
        # 텍스트 박스 검색어 입력후 엔터를 피면 처리
        self.txtSearch.returnPressed.connect(self.txtSearchReturned)


    def btnSearchClicked(self):
        search = self.txtSearch.text()

        if search == '':
            QMessageBox.critical(self, '경고', '검색어를 입력하세요!')
            return
        else:
            api = NaverApi() # NaverApi 클래스 객체 생성
            result = []
            node = 'news' # movie로 변경하면 영화검색
            outputs = [] # 결과 담을 리스트 변수
            display = 100 # 100개만 출력

            result = api.get_naver_search(node, search, 1, display)
            # print(result)
            # 테이블 위젯에 출력가능
            items = result['items'] # json결좌 중 items부분만 잘라서 때겠다.   
            #print(len(items))   
            self.makeTable(items) # 테이블위젯에 데이터들을 할당함수      

    # 테이블 위젯에 데이터 표시
    def makeTable(self, items) -> None:
        self.tblResult.setColumnCount(3) # 테이블 행을 세주는 것
        self.tblResult.setRowCount(len(items)) # 밑으로 몇개의 행을 만들어 줄것인지

        for i, post in enumerate(items): # 0, 뉴스...
            num = i +1 # 0부터 시작하기 때문에
            title = post['title']
            originallink = post['originallink']
            # setItem(행, 열, 넣을데이터)
            #self.tblResult.setItem(i, 0, QTableWidgetItem(str(num)))
            self.tblResult.setItem(i, 0, QTableWidgetItem(title))
            self.tblResult.setItem(i, 1, QTableWidgetItem(originallink))

    def txtSearchReturned(self):
           self.btnSearchClicked()
            
   

if __name__ == '__main__':
    app = QApplication(sys.argv)
    ex = qtApp()
    ex.show()
    sys.exit(app.exec_())