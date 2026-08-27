~~1. Diminuer la taille des textbox dans les éditeurs pour que le bouton apparaisse.~~  
~~2. Passer la gestion du staff d'un combobox à une grid~~  
3. Connexion hors zone géographique autorisée  
    - Vérification de la localisation absente lors de la connexion.  
    - Le popup permettant à l'utilisateur de décider s'il souhaite continuer n'est pas implémenté.  
4. Fonctionnement hors ligne  
    - Le cahier des charges prévoit un fonctionnement avec des données stockées localement.  
    - L'utilisation actuelle de PostgreSQL sur Azure rend les données dépendantes d'une connexion réseau.  
    - Aucun mécanisme de cache ou de stockage local n'est actuellement prévu.  
5. Statistiques d'utilisation  
    - Les statistiques sont définies comme une fonctionnalité métier.  
    - L'administration doit permettre leur consultation dans le MVP.  
    - AdminViewModel ne gère actuellement que les utilisateurs.  
    - Aucune collecte ni affichage de statistiques d'utilisation n'est présent.  
6. Filtrage de l'agenda  
    - Le MVP prévoit la consultation et le filtrage des événements.  
    - AgendaViewModel permet actuellement d'afficher les événements, mais pas de les filtrer.  
7. Association d'un gestionnaire de contenu à un musée  
    - Le cahier des charges impose qu'un gestionnaire ne puisse modifier que les musées auxquels il est associé.  
    - User ne contient actuellement aucune association avec un ou plusieurs musées.  
    - ContentManagerViewModel charge les musées sans filtrage selon l'utilisateur connecté.  
    - Un gestionnaire peut donc actuellement modifier les contenus de n'importe quel musée.  
    - Restriction d'âge pour la collecte de données non essentielles
Une restriction est prévue pour les utilisateurs mineurs.  
    - Aucune vérification d'âge n'est actuellement présente dans les formulaires concernés, notamment newsletter/satisfaction.  
8. Éditeur d'étage  
    - FloorEditorViewModel semble ne pas avoir été réellement implémenté.  
    - La modification du contenu d'un étage n'est donc pas disponible, contrairement aux éditeurs de Space, PointOfInterest et Museum.  
9. Gestion de plusieurs musées  
    - Le document prévoit la possibilité de gérer plusieurs musées.  
    - MapViewModel et ContentManagerViewModel utilisent actuellement systématiquement le premier musée trouvé avec FirstOrDefault().  
    - Il n'existe pas encore de sélection ou de bascule entre plusieurs musées.  
10. Faire plan moins chargé et en SVG
11. Ajouter beaucoup de données
12. Modifier l'affichage d'un point d'intérêt pour y inclure une image