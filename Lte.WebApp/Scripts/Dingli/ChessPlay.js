
function ChessPiece() {
    this.image = null;
    this.x = 0;
    this.y = 0;
    this.height = 0;
    this.width = 0;
    this.killed = true;
    this.generator = new ChessPieceGenerator();
}

function ChessPieceGenerator() {

    //Define the chess piece images
    this.imgPawn = new Image();
    this.imgRook = new Image();
    this.imgKnight = new Image();
    this.imgBishop = new Image();
    this.imgQueen = new Image();
    this.imgQueen = new Image();
    this.imgKing = new Image();
    this.imgPawnW = new Image();
    this.imgRookW = new Image();
    this.imgKnightW = new Image();
    this.imgBishopW = new Image();
    this.imgQueenW = new Image();
    this.imgQueenW = new Image();
    this.imgKingW = new Image();
    this.imgPawn.src = "/Images/pawn.png";
    this.imgRook.src = "/Images/rook.png";
    this.imgKnight.src = "/Images/knight.png";
    this.imgBishop.src = "/Images/bishop.png";
    this.imgQueen.src = "/Images/queen.png";
    this.imgKing.src = "/Images/king.png";
    this.imgPawnW.src = "/Images/wpawn.png";
    this.imgRookW.src = "/Images/wrook.png";
    this.imgKnightW.src = "/Images/wknight.png";
    this.imgBishopW.src = "/Images/wbishop.png";
    this.imgQueenW.src = "/Images/wqueen.png";
    this.imgKingW.src = "/Images/wking.png";

}

ChessPiece.prototype.drawPawn = function(index) {
    this.image = this.generator.imgPawn;
    this.x = index;
    this.y = 1;
    this.height = 50;
    this.width = 28;
    this.killed = false;
};

ChessPiece.prototype.drawRook = function(x) {
    this.image = this.generator.imgRook;
    this.x = x;
    this.y = 0;
    this.height = 60;
    this.width = 36;
    this.killed = false;
};

ChessPiece.prototype.drawKnight = function(x) {
    this.image = this.generator.imgKnight;
    this.x = x;
    this.y = 0;
    this.height = 60;
    this.width = 36;
    this.killed = false;
};

ChessPiece.prototype.drawBishop = function(x) {
    this.image = this.generator.imgBishop;
    this.x = x;
    this.y = 0;
    this.height = 65;
    this.width = 30;
    this.killed = false;
};

ChessPiece.prototype.drawQueen = function() {
    this.image = this.generator.imgQueen;
    this.x = 3;
    this.y = 0;
    this.height = 70;
    this.width = 28;
    this.killed = false;
};

ChessPiece.prototype.drawKing = function() {
    this.image = this.generator.imgKing;
    this.x = 4;
    this.y = 0;
    this.height = 70;
    this.width = 28;
    this.killed = false;
};

ChessPiece.prototype.drawPawnW = function (index) {
    this.image = this.generator.imgPawnW;
    this.x = index;
    this.y = 6;
    this.height = 50;
    this.width = 28;
    this.killed = false;
};

ChessPiece.prototype.drawRookW = function (x) {
    this.image = this.generator.imgRookW;
    this.x = x;
    this.y = 7;
    this.height = 60;
    this.width = 36;
    this.killed = false;
};

ChessPiece.prototype.drawKnightW = function (x) {
    this.image = this.generator.imgKnightW;
    this.x = x;
    this.y = 7;
    this.height = 60;
    this.width = 36;
    this.killed = false;
};

ChessPiece.prototype.drawBishopW = function (x) {
    this.image = this.generator.imgBishopW;
    this.x = x;
    this.y = 7;
    this.height = 65;
    this.width = 30;
    this.killed = false;
};

ChessPiece.prototype.drawQueenW = function () {
    this.image = this.generator.imgQueenW;
    this.x = 3;
    this.y = 7;
    this.height = 70;
    this.width = 28;
    this.killed = false;
};

ChessPiece.prototype.drawKingW = function () {
    this.image = this.generator.imgKingW;
    this.x = 4;
    this.y = 7;
    this.height = 70;
    this.width = 28;
    this.killed = false;
};

