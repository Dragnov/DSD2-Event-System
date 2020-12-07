# DSD2-Event-System

## Description

Don't stop digger2를 작업하면서 이벤트 사용을 용이하게 하려는 의도로 작성한 코드입니다.

시작은 [csHandler_Event](https://github.com/Dragnov/DSD2-Event-System/blob/main/csHandler_Event.cs)를 [상속받은 클래스](https://github.com/Dragnov/DSD2-Event-System/blob/main/csHandler_Event_Sample.cs)를 생성하여 사용할 이벤트 필드를 선언해줍니다.

필드를 선언할 때 선언 가능한 이벤트 타입은 다음과 같습니다.

|이벤트|용도|접두어|
|-|-|-|
|[csState](https://github.com/Dragnov/DSD2-Event-System/blob/main/csState.cs)|상태 시작 및 종료|OnStart_, OnStop_|
|[csSender](https://github.com/Dragnov/DSD2-Event-System/blob/main/csSender.cs)|반환값이 없는 이벤트 호출|OnSend_|
|[csReturner](https://github.com/Dragnov/DSD2-Event-System/blob/main/csReturner.cs)|반환값이 있는 이벤트 호출|OnReturn_|

다음은 
