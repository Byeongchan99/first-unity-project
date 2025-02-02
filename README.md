# first-unity-project
# 프로젝트 소개

개인 프로젝트로 제작한 Unity 기반 탑다운 2D 액션 로그라이트 게임입니다.

플레이어는 몬스터들을 처치해 모든 지역을 정화하고, 각 지역의 봉인석을 복구해야 합니다. 폭주한 마을 수호자인 골렘을 무찌르고 마을에 평화를 되찾아주세요.

Window로 빌드되어 아래 사이트에서 다운받아 직접 플레이해보실 수 있습니다.

</br>

# 세부 사항

### 기간

2023.06.06 ~ 2024.01.11

### 주요 업무

게임의 기획, 도트 그래픽 스프라이트 제작, UI/UX 디자인 및 프로그래밍을 포함한 모든 개발 과정을 직접 담당하였습니다.

### itch.io 사이트

[룬 크로니클](https://harrrypoter.itch.io/rune-chronicle)

### GitHub Repository

https://github.com/Byeongchan99/first-unity-project

### 플레이 영상

[룬 크로니클 플레이 영상](https://youtu.be/1B79nPlY3kg?si=RU_ph8WGZINtLdmP)

</br>

# 게임 구조

![Untitled](https://github.com/user-attachments/assets/93893b0b-7f63-4c03-9e7b-372d212e82e7)

게임의 전체적인 구조 및 흐름입니다.

플레이어는 일반적으로 메인 씬에서 조작하게 되며, 메인 화면으로 돌아가거나 게임을 재시작하려 할 때 로딩 씬을 이용하게 됩니다.

플레이어는 로드아웃에서 선택한 무기로 게임을 진행하여 일반 스테이지들을 모두 클리어 시 보스 스테이지에 진입할 수 있습니다. 만약 체력을 모두 잃게되면 게임 오버가 되고, 보스 몬스터를 처치 시 게임을 클리어하게 됩니다.

메인 스테이지의 상점에서 체력 회복과 어빌리티 구매를 통해 능력치를 강화하여 게임을 클리어하는 것이 목적입니다.

</br>

# 주요 기능

1. **플레이어 근접 공격 구현**

</br>

![PlayerAttack-ezgif com-resize](https://github.com/user-attachments/assets/89d706dd-75cd-4833-9762-60a7233e4d84)

</br>

[룬 크로니클 개발 일지 7 - 공격 로직 및 무기 구현(1)](https://www.notion.so/7-1-0432376d0f0d4fd4a56f20c7104dcb8d?pvs=21) 

</br>

2. **일반 몬스터 구현**

</br>

![ChaseState](https://github.com/user-attachments/assets/f78ffbb9-fb10-4d19-97b0-5871953ae22e)

</br>

[룬 크로니클 개발 일지 12 - 일반 몬스터](https://www.notion.so/12-3e4accb6fc904a8693b47f92cda9f706?pvs=21) 

</br>

3. **보스 몬스터 구현**

</br>

![BossPattern4-ezgif com-crop](https://github.com/user-attachments/assets/0589c770-71e9-4852-a30d-56881169b30e)

</br>

[룬 크로니클 개발 일지 13 - 보스 몬스터](https://www.notion.so/13-5861509670d245d89bd742d0a0ced42e?pvs=21)

</br>

# 프로젝트 경험

</br>

[룬 크로니클 개발 개선점 및 아쉬웠던 점](https://www.notion.so/8f98589622f847819067b1ada06180fe?pvs=21) 

</br>

- 플레이어
    - 유니티의 새로운 인풋 시스템 다루기
    - 상태 패턴 적용하기
    - 애니메이터와 애니메이션 다루기
    - PlayerPref로 유저 정보 저장하기

</br>

- 일반 몬스터
    - A* 알고리즘으로 추적 로직 구현하기
    - 간단한 상태 패턴 적용하기

</br>

- 보스 몬스터
    - 코루틴으로 여러 공격 패턴 구현하기

</br>

- 스테이지
    - 스크립터블 오브젝트로 몬스터 웨이브 정보 저장하기
    - 프리펩으로 스테이지 관리하기
    - 타일맵으로 스테이지 구현하기
    - 레이어 이용하여 층 구현하기

</br>

- 이벤트 및 연출
    - 유니티 타임라인 기능 사용하여 이벤트 컷씬 제작하기
    - 비동기 호출로 로딩 화면 제작하기

</br>

- 이펙트 및 사운드
    - 오브젝트 풀로 몬스터와 이펙트, 파티클 관리하기
    - 오디오 매니저로 오디오 소스와 오디오 클립 관리하기

</br>

- 아트
    - 픽셀 아트 드로잉 툴인 Aseprite를 이용하여 애니메이션, 스프라이트 제작하기

</br>
