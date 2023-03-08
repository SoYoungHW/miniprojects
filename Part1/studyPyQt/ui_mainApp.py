from PyQt5 import QtCore, QtGui, QtWidgets

class Ui_Form(object):
    def setupUi(self, Form):
        Form.setObjectName("From")
        Form.resize(281, 162)
        sizePolicy = QtWidgets.QSizePolicy(QtWidgets.QSizePolicy.Fixed, QtWidgets.QSizePolicy.fixed)
        sizePolicy.setHorizontalStretch(0)
        sizePolicy.setVerticalStretch(0)
        sizePolicy.setHeightForWidth(Form.sizePolicy().hasHeightForWidth())
        Form.setSizePolicy(sizePolicy)
        Form.setMinimumSize(QtCore.QSize(281, 162))
        Form.setMaximumSize(QtCore.QSize(281, 162))
        font = QtGui.QFont()
        font.setFamily("나눔고딕")
        Form.setFont(font)
        self.lblMessage = QtWidgets.QLabel(Form)
        self.lblMessage.setGeometry(QtCore.QRect(10, 20, 261, 21))
        self.lblMessage.setObjectName("lblMessage")
        self.btnOK = QtWidgets.QPushButton(Form)
        self.btnOK.setGeometry(QtCore.QRect(190, 120, 81, 31))
        self.btnOK.setObjectName("btnOK")
        self.btnPOP = QtWidgets.QPushButton(Form)
        self.btnPOP.setGeometry(QtCore.QRect(100, 120, 81, 31))
        self.btnPOP.setObjectName("btnPOP")

        self.retranslateUi(Form)
        QtCore.QMetaObject.connectSlotsByName(Form)

    def retranslateUi(self, Form):
        _translate = QtCore.QCoreApplication.translate
        Form.setWindowTitle(_translate("Form", "메인앱"))
        self.lblMessage.setText(_translate("Form", "메시지 : "))
        self.btnOK.setText(_translate("Form", "OK"))
        self.btnPOP.setText(_translate("Form", "Popup"))